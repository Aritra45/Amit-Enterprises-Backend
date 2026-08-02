using Microsoft.Extensions.Caching.Memory;
using Shared.Core.Abstractions;

namespace Shared.Infrastructure.Identity;

public class TokenBlacklistService : ITokenBlacklistService
{
    private const string CacheKeyPrefix = "blacklisted-jwt:";

    private readonly IMemoryCache _cache;

    public TokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Blacklist(string jti, DateTime expiresAtUtc)
    {
        var ttl = expiresAtUtc - DateTime.UtcNow;
        if (ttl <= TimeSpan.Zero)
        {
            return;
        }

        _cache.Set(CacheKeyPrefix + jti, true, ttl);
    }

    public bool IsBlacklisted(string jti) => _cache.TryGetValue(CacheKeyPrefix + jti, out _);
}
