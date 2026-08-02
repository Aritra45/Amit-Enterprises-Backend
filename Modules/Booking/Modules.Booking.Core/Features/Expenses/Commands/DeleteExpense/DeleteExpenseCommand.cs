using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Commands.DeleteExpense;

public record DeleteExpenseCommand(int Id) : IRequest<IResult>;
