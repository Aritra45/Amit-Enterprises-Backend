namespace Shared.Core.Abstractions;

/// <summary>
/// Tracks access-token ids (jti) revoked before their natural expiry (e.g. on logout),
/// since a signed JWT can't otherwise be invalidated before it expires.
/// </summary>
public interface ITokenBlacklistService
{
    void Blacklist(string jti, DateTime expiresAtUtc);

    bool IsBlacklisted(string jti);
}
