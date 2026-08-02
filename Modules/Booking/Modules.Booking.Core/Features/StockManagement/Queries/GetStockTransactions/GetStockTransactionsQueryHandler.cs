using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.StockManagement.Queries.GetStockTransactions;

public class GetStockTransactionsQueryHandler : IRequestHandler<GetStockTransactionsQuery, PaginatedResult<StockTransactionResponse>>
{
    private readonly IStockTransactionRepository _stockTransactionRepository;

    public GetStockTransactionsQueryHandler(IStockTransactionRepository stockTransactionRepository)
    {
        _stockTransactionRepository = stockTransactionRepository;
    }

    public async Task<PaginatedResult<StockTransactionResponse>> Handle(GetStockTransactionsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _stockTransactionRepository.GetPagedAsync(
            request.ProductId,
            request.TransactionType,
            request.FromDate,
            request.ToDate,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mapped = items.Select(t => new StockTransactionResponse
        {
            Id = t.Id,
            ProductId = t.ProductId,
            ProductName = t.ProductName,
            TransactionType = t.TransactionType,
            QuantityChange = t.QuantityChange,
            StockAfterTransaction = t.StockAfterTransaction,
            ReferenceNumber = t.ReferenceNumber,
            Remarks = t.Remarks,
            CreatedOn = t.CreatedOn
        }).ToList();

        return PaginatedResult<StockTransactionResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
