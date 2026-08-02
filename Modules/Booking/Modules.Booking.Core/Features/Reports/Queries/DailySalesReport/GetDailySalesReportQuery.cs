using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.DailySalesReport;

public record GetDailySalesReportQuery(DateTime FromDate, DateTime ToDate) : IRequest<Result<List<DailySalesReportItem>>>;
