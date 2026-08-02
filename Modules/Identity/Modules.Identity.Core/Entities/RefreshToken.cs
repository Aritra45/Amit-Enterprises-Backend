using Shared.Core.Entities;

namespace Modules.Identity.Core.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? RevokedOn { get; set; }

    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

    public bool IsValid => !IsRevoked && !IsExpired;
}
