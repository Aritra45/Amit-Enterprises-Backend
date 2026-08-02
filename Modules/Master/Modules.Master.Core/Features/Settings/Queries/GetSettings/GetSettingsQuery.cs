using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Queries.GetSettings;

public record GetSettingsQuery : IRequest<Result<SettingsResponse>>;
