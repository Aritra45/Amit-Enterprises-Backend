using FluentValidation;

namespace Modules.Master.Core.Features.Products.Commands.UploadProductImage;

public class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FileStream).NotNull();
    }
}
