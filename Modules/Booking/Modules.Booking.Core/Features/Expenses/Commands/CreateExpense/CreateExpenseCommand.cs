using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(string Title, string? Category, double Amount, DateTime ExpenseDate, string? Notes) : IRequest<Result<int>>;
