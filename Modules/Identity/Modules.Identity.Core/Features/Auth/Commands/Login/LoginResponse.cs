namespace Modules.Identity.Core.Features.Auth.Commands.Login;

public class LoginResponse
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public DateTime AccessTokenExpiresOn { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
}
