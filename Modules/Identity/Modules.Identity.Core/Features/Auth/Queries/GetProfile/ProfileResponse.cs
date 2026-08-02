namespace Modules.Identity.Core.Features.Auth.Queries.GetProfile;

public class ProfileResponse
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? MobileNumber { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
}
