using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string ProductCode,
    string? Barcode,
    string ProductName,
    int CategoryId,
    double PurchasePrice,
    double SellingPrice,
    double GSTPercentage,
    double MinStockQty,
    string? ProductImage) : IRequest<IResult>;
