using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ProductSalesReport;

public record GetProductSalesReportQuery(DateTime FromDate, DateTime ToDate) : IRequest<Result<List<ProductSalesReportItem>>>;
