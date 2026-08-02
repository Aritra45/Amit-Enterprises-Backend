using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Product", request.Id);

        if (await _productRepository.ProductCodeExistsAsync(request.ProductCode, request.Id, cancellationToken))
        {
            throw new ConflictException($"Product code '{request.ProductCode}' already exists.");
        }

        if (!string.IsNullOrWhiteSpace(request.Barcode)
            && await _productRepository.BarcodeExistsAsync(request.Barcode, request.Id, cancellationToken))
        {
            throw new ConflictException($"Barcode '{request.Barcode}' is already assigned to another product.");
        }

        product.ProductCode = request.ProductCode;
        product.Barcode = request.Barcode;
        product.ProductName = request.ProductName;
        product.CategoryId = request.CategoryId;
        product.PurchasePrice = request.PurchasePrice;
        product.SellingPrice = request.SellingPrice;
        product.GSTPercentage = request.GSTPercentage;
        product.MinStockQty = request.MinStockQty;
        product.ProductImage = request.ProductImage;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Product updated successfully.");
    }
}
