namespace Shared.Core.Abstractions;

public record AccessTokenResult(string Token, string Jti, DateTime ExpiresAtUtc);

public interface IJwtService
{
    AccessTokenResult GenerateAccessToken(int userId, string userName, string role);

    string GenerateRefreshToken();

    int RefreshTokenExpirationDays { get; }
}
