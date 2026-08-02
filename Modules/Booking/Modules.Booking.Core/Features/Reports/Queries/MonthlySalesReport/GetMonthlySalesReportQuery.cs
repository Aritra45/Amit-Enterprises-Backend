using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.MonthlySalesReport;

public record GetMonthlySalesReportQuery(int Year) : IRequest<Result<List<MonthlySalesReportItem>>>;
