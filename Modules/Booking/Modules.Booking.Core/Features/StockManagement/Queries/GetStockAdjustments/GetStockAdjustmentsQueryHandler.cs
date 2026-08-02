using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.StockManagement.Queries.GetStockAdjustments;

public class GetStockAdjustmentsQueryHandler : IRequestHandler<GetStockAdjustmentsQuery, PaginatedResult<StockAdjustmentResponse>>
{
    private readonly IStockAdjustmentRepository _stockAdjustmentRepository;

    public GetStockAdjustmentsQueryHandler(IStockAdjustmentRepository stockAdjustmentRepository)
    {
        _stockAdjustmentRepository = stockAdjustmentRepository;
    }

    public async Task<PaginatedResult<StockAdjustmentResponse>> Handle(GetStockAdjustmentsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _stockAdjustmentRepository.GetPagedAsync(
            request.ProductId,
            request.AdjustmentType,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mapped = items.Select(a => new StockAdjustmentResponse
        {
            Id = a.Id,
            ProductId = a.ProductId,
            ProductName = a.ProductName,
            AdjustmentType = a.AdjustmentType,
            Quantity = a.Quantity,
            IsIncrease = a.IsIncrease,
            Reason = a.Reason,
            AdjustmentDate = a.AdjustmentDate
        }).ToList();

        return PaginatedResult<StockAdjustmentResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
