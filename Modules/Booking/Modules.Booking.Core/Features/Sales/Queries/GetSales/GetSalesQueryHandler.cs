using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Sales.Queries.GetSales;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, PaginatedResult<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<PaginatedResult<SaleResponse>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _saleRepository.GetPagedAsync(
            request.FromDate,
            request.ToDate,
            request.SearchTerm,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mapped = items.Select(sale => new SaleResponse
        {
            Id = sale.Id,
            InvoiceNumber = sale.InvoiceNumber,
            SaleDate = sale.SaleDate,
            SubTotal = sale.SubTotal,
            DiscountAmount = sale.DiscountAmount,
            GSTAmount = sale.GSTAmount,
            GrandTotal = sale.GrandTotal,
            CustomerName = sale.CustomerName,
            CustomerMobile = sale.CustomerMobile,
            PaymentMode = sale.PaymentMode,
            Items = sale.SaleItems.Select(i => new SaleItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                GSTPercentage = i.GSTPercentage,
                DiscountAmount = i.DiscountAmount,
                GSTAmount = i.GSTAmount,
                TotalAmount = i.TotalAmount
            }).ToList()
        }).ToList();

        return PaginatedResult<SaleResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
