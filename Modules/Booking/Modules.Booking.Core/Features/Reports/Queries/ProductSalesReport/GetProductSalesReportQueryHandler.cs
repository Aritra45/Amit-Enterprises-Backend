using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ProductSalesReport;

public class GetProductSalesReportQueryHandler : IRequestHandler<GetProductSalesReportQuery, Result<List<ProductSalesReportItem>>>
{
    private readonly ISaleRepository _saleRepository;

    public GetProductSalesReportQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<List<ProductSalesReportItem>>> Handle(GetProductSalesReportQuery request, CancellationToken cancellationToken)
    {
        var projections = await _saleRepository.GetProductSalesAsync(request.FromDate, request.ToDate, cancellationToken);

        var items = projections.Select(p => new ProductSalesReportItem
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            ProductCode = p.ProductCode,
            QuantitySold = p.QuantitySold,
            Revenue = p.Revenue
        }).ToList();

        return Result<List<ProductSalesReportItem>>.Success(items);
    }
}
