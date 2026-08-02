using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.UpdateExpense;

public record UpdateExpenseCommand(int Id, string Title, string? Category, double Amount, DateTime ExpenseDate, string? Notes) : IRequest<IResult>;
