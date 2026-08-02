using MediatR;
using Modules.Booking.Core.Entities;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Booking.Core.Features.StockManagement.Queries.GetStockAdjustments;

public class GetStockAdjustmentsQuery : PaginationRequest, IRequest<PaginatedResult<StockAdjustmentResponse>>
{
    public int? ProductId { get; set; }

    public StockAdjustmentType? AdjustmentType { get; set; }
}
