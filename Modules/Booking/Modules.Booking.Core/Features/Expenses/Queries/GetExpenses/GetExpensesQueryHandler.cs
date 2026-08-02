using AutoMapper;
using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Expenses.Queries.GetExpenses;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, PaginatedResult<ExpenseResponse>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpensesQueryHandler(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ExpenseResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _expenseRepository.GetPagedAsync(
            request.FromDate,
            request.ToDate,
            request.Category,
            request.SearchTerm,
            request.SortColumn,
            request.SortDescending,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mapped = _mapper.Map<List<ExpenseResponse>>(items);

        return PaginatedResult<ExpenseResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
