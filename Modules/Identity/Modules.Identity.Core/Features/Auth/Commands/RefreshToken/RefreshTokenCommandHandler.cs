using MediatR;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Features.Auth.Commands.Login;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenWithUserAsync(request.RefreshToken, cancellationToken);

        if (existingToken is null || !existingToken.IsValid || !existingToken.User.IsActive)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        var user = existingToken.User;

        var newAccessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.Name);
        var newRefreshTokenValue = _jwtService.GenerateRefreshToken();

        existingToken.IsRevoked = true;
        existingToken.RevokedOn = DateTime.UtcNow;
        existingToken.ReplacedByToken = newRefreshTokenValue;
        _refreshTokenRepository.Update(existingToken);

        var newRefreshToken = new Entities.RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshTokenValue,
            ExpiresOn = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenExpirationDays)
        };
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            AccessToken = newAccessToken.Token,
            AccessTokenExpiresOn = newAccessToken.ExpiresAtUtc,
            RefreshToken = newRefreshTokenValue
        };

        return Result<LoginResponse>.Success(response);
    }
}
