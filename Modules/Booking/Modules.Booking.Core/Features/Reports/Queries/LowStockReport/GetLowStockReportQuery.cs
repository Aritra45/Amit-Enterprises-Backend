using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.LowStockReport;

public record GetLowStockReportQuery : IRequest<Result<List<LowStockReportItem>>>;
