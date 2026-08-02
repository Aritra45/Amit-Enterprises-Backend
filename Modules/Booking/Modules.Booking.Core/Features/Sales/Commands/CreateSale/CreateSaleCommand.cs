using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Sales.Commands.CreateSale;

public record SaleItemRequest(int ProductId, double Quantity, double DiscountAmount);

public record CreateSaleCommand(
    List<SaleItemRequest> Items,
    double DiscountAmount,
    string? CustomerName,
    string? CustomerMobile,
    string PaymentMode) : IRequest<Result<SaleResponse>>;
