using FluentValidation;

namespace Modules.Booking.Core.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
