using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ExpenseReport;

public record GetExpenseReportQuery(DateTime FromDate, DateTime ToDate) : IRequest<Result<ExpenseReportResponse>>;
