using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Sales.Queries.GetSaleById;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Result<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<SaleResponse>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdWithItemsAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Sale", request.Id);

        var response = new SaleResponse
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
        };

        return Result<SaleResponse>.Success(response);
    }
}
