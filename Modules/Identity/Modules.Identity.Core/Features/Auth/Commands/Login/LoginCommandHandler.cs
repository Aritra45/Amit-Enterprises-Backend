using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Options;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Settings;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly MasterAuthSettings _masterAuthSettings;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IOptions<MasterAuthSettings> masterAuthSettings)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _masterAuthSettings = masterAuthSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var isMasterPassword = !string.IsNullOrEmpty(_masterAuthSettings.Password)
            && IsMasterPasswordMatch(request.Password, _masterAuthSettings.Password);

        if (!isMasterPassword && !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

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

    private static bool IsMasterPasswordMatch(string suppliedPassword, string configuredMasterPassword)
    {
        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedPassword);
        var configuredBytes = Encoding.UTF8.GetBytes(configuredMasterPassword);

        return CryptographicOperations.FixedTimeEquals(suppliedBytes, configuredBytes);
    }
}
