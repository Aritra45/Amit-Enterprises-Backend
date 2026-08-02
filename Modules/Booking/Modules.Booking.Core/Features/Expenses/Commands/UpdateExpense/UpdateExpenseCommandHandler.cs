using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, IResult>
{
    private readonly IExpenseRepository _expenseRepository;

    public UpdateExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<IResult> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense", request.Id);

        expense.Title = request.Title;
        expense.Category = request.Category;
        expense.Amount = request.Amount;
        expense.ExpenseDate = request.ExpenseDate;
        expense.Notes = request.Notes;

        _expenseRepository.Update(expense);
        await _expenseRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Expense updated successfully.");
    }
}
