using MediatR;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Entities;
using Modules.Identity.Core.Features.Auth.Commands.Login;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(request.Email, null, cancellationToken))
        {
            throw new ConflictException("A user with this email already exists.");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            throw new NotFoundException("Role", request.RoleId);
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            MobileNumber = request.MobileNumber,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = role.Id,
            IsActive = true,
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        user.Role = role;

        var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.Name);
        var refreshTokenValue = _jwtService.GenerateRefreshToken();

        var refreshToken = new Entities.RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresOn = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenExpirationDays)
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            AccessToken = accessToken.Token,
            AccessTokenExpiresOn = accessToken.ExpiresAtUtc,
            RefreshToken = refreshTokenValue
        };

        return Result<LoginResponse>.Success(response);
    }
}
