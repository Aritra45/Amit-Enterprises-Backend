using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _productRepository.ProductCodeExistsAsync(request.ProductCode, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Product code '{request.ProductCode}' already exists.");
        }

        if (!string.IsNullOrWhiteSpace(request.Barcode)
            && await _productRepository.BarcodeExistsAsync(request.Barcode, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Barcode '{request.Barcode}' is already assigned to another product.");
        }

        var product = new Entities.Product
        {
            ProductCode = request.ProductCode,
            Barcode = request.Barcode,
            ProductName = request.ProductName,
            CategoryId = request.CategoryId,
            PurchasePrice = request.PurchasePrice,
            SellingPrice = request.SellingPrice,
            GSTPercentage = request.GSTPercentage,
            CurrentStockQty = request.CurrentStockQty,
            MinStockQty = request.MinStockQty,
            ProductImage = request.ProductImage
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(product.Id, "Product created successfully.");
    }
}
