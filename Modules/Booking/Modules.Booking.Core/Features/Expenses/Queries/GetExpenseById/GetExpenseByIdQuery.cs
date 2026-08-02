using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Queries.GetExpenseById;

public record GetExpenseByIdQuery(int Id) : IRequest<Result<ExpenseResponse>>;
