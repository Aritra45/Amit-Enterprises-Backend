using MediatR;
using Modules.Master.Core.Features.Settings;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Commands.UpdateSettings;

public record UpdateSettingsCommand(
    string ShopName,
    string OwnerName,
    string? MobileNumber,
    string? Address,
    string? GSTNumber,
    string? LogoUrl) : IRequest<Result<SettingsResponse>>;
