using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, IResult>
{
    private readonly IExpenseRepository _expenseRepository;

    public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<IResult> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense", request.Id);

        _expenseRepository.Remove(expense);
        await _expenseRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Expense deleted successfully.");
    }
}
