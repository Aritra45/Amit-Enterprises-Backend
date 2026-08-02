using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ProfitReport;

public record GetProfitReportQuery(DateTime FromDate, DateTime ToDate) : IRequest<Result<ProfitReportResponse>>;
