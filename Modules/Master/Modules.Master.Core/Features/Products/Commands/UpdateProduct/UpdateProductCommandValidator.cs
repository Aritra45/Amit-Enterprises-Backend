using FluentValidation;
using Modules.Master.Core.Abstractions;

namespace Modules.Master.Core.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.ProductCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Barcode)
            .MaximumLength(50);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .MustAsync(async (categoryId, cancellationToken) => await categoryRepository.AnyAsync(c => c.Id == categoryId, cancellationToken))
            .WithMessage("The selected category does not exist.");

        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);

        RuleFor(x => x.SellingPrice).GreaterThanOrEqualTo(0);

        RuleFor(x => x.GSTPercentage).InclusiveBetween(0, 100);

        RuleFor(x => x.MinStockQty).GreaterThanOrEqualTo(0);
    }
}
