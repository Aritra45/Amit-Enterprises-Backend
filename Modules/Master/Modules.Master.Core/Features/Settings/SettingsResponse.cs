namespace Modules.Master.Core.Features.Settings;

public class SettingsResponse
{
    public int Id { get; set; }

    public string ShopName { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public string? MobileNumber { get; set; }

    public string? Address { get; set; }

    public string? GSTNumber { get; set; }

    public string? LogoUrl { get; set; }
}
