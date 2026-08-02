using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Dashboard;

public record GetDashboardQuery : IRequest<Result<DashboardResponse>>;
