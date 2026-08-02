using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.UploadProductImage;

public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, Result<string>>
{
    private const string Folder = "products";

    private readonly IFileStorageService _fileStorageService;
    private readonly IProductRepository _productRepository;

    public UploadProductImageCommandHandler(IFileStorageService fileStorageService, IProductRepository productRepository)
    {
        _fileStorageService = fileStorageService;
        _productRepository = productRepository;
    }

    public async Task<Result<string>> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var url = await _fileStorageService.UploadImageAsync(request.FileStream, request.FileName, Folder, cancellationToken);

        if (request.ProductId.HasValue)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId.Value, cancellationToken)
                ?? throw new NotFoundException("Product", request.ProductId.Value);

            product.ProductImage = url;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);
        }

        return Result<string>.Success(url, "Image uploaded successfully.");
    }
}
