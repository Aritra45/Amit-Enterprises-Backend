using FluentValidation;
using Shared.Core.Abstractions;

namespace Modules.Booking.Core.Features.StockManagement.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentCommandValidator : AbstractValidator<CreateStockAdjustmentCommand>
{
    public CreateStockAdjustmentCommandValidator(IProductCatalogService productCatalogService)
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .MustAsync(async (productId, cancellationToken) => await productCatalogService.ExistsAsync(productId, cancellationToken))
            .WithMessage("The selected product does not exist.");

        RuleFor(x => x.Quantity).GreaterThan(0);

        RuleFor(x => x.AdjustmentType).IsInEnum();
    }
}
