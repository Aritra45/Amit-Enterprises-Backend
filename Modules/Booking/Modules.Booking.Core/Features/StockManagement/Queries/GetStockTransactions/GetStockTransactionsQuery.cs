using MediatR;
using Modules.Booking.Core.Entities;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Booking.Core.Features.StockManagement.Queries.GetStockTransactions;

public class GetStockTransactionsQuery : PaginationRequest, IRequest<PaginatedResult<StockTransactionResponse>>
{
    public int? ProductId { get; set; }

    public StockTransactionType? TransactionType { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
