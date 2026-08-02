using MediatR;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Result<int>>
{
    private readonly IExpenseRepository _expenseRepository;

    public CreateExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<int>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Title = request.Title,
            Category = request.Category,
            Amount = request.Amount,
            ExpenseDate = request.ExpenseDate,
            Notes = request.Notes
        };

        await _expenseRepository.AddAsync(expense, cancellationToken);
        await _expenseRepository.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(expense.Id, "Expense recorded successfully.");
    }
}
