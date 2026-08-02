using MediatR;
using Shared.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.LowStockReport;

public class GetLowStockReportQueryHandler : IRequestHandler<GetLowStockReportQuery, Result<List<LowStockReportItem>>>
{
    private const int MaxRows = 10_000;

    private readonly IProductCatalogService _productCatalogService;

    public GetLowStockReportQueryHandler(IProductCatalogService productCatalogService)
    {
        _productCatalogService = productCatalogService;
    }

    public async Task<Result<List<LowStockReportItem>>> Handle(GetLowStockReportQuery request, CancellationToken cancellationToken)
    {
        var lowStockProducts = await _productCatalogService.GetLowStockProductsAsync(MaxRows, cancellationToken);

        var items = lowStockProducts.Select(p => new LowStockReportItem
        {
            ProductId = p.Id,
            ProductCode = p.ProductCode,
            ProductName = p.ProductName,
            CurrentStockQty = p.CurrentStockQty,
            MinStockQty = p.MinStockQty
        }).ToList();

        return Result<List<LowStockReportItem>>.Success(items);
    }
}
