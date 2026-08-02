using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.UploadProductImage;

/// <summary>If ProductId is supplied the uploaded image is also saved onto that product; otherwise just the URL is returned (useful during Create, before the product exists).</summary>
public record UploadProductImageCommand(int? ProductId, Stream FileStream, string FileName) : IRequest<Result<string>>;
