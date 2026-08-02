using MediatR;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, IResult>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenBlacklistService _tokenBlacklistService;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, ITokenBlacklistService tokenBlacklistService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenBlacklistService = tokenBlacklistService;
    }

    public async Task<IResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenWithUserAsync(request.RefreshToken, cancellationToken);

        if (existingToken is not null && existingToken.IsValid)
        {
            existingToken.IsRevoked = true;
            existingToken.RevokedOn = DateTime.UtcNow;
            _refreshTokenRepository.Update(existingToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(request.AccessTokenJti) && request.AccessTokenExpiresOnUtc.HasValue)
        {
            _tokenBlacklistService.Blacklist(request.AccessTokenJti, request.AccessTokenExpiresOnUtc.Value);
        }

        return Result.Success("Logged out successfully.");
    }
}
