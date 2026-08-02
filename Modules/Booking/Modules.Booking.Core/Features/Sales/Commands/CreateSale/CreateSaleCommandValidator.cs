using FluentValidation;

namespace Modules.Booking.Core.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one sale item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty();
            item.RuleFor(i => i.Quantity).GreaterThan(0);
            item.RuleFor(i => i.DiscountAmount).GreaterThanOrEqualTo(0);
        });

        RuleFor(x => x.DiscountAmount).GreaterThanOrEqualTo(0);

        RuleFor(x => x.PaymentMode).NotEmpty();

        RuleFor(x => x.CustomerMobile)
            .Matches(@"^\d{10}$")
            .When(x => !string.IsNullOrWhiteSpace(x.CustomerMobile))
            .WithMessage("Customer mobile number must be exactly 10 digits.");
    }
}
