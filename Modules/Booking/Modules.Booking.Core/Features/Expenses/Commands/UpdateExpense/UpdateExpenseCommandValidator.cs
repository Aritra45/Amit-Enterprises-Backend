using FluentValidation;

namespace Modules.Booking.Core.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

        RuleFor(x => x.Category).MaximumLength(100);

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.ExpenseDate).NotEmpty();
    }
}
