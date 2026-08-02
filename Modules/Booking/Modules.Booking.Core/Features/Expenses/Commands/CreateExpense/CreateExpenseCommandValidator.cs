using FluentValidation;

namespace Modules.Booking.Core.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

        RuleFor(x => x.Category).MaximumLength(100);

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.ExpenseDate).NotEmpty();
    }
}
