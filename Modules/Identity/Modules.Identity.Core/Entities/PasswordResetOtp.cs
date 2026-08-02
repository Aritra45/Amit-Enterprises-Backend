using Shared.Core.Entities;

namespace Modules.Identity.Core.Entities;

public class PasswordResetOtp : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string OtpHash { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }

    public bool IsVerified { get; set; }

    public bool IsUsed { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
}
