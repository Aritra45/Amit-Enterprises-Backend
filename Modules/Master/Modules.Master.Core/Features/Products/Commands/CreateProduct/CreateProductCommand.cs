using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string ProductCode,
    string? Barcode,
    string ProductName,
    int CategoryId,
    double PurchasePrice,
    double SellingPrice,
    double GSTPercentage,
    double CurrentStockQty,
    double MinStockQty,
    string? ProductImage) : IRequest<Result<int>>;
