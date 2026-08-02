using MediatR;
using Modules.Booking.Core.Entities;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.StockManagement.Commands.CreateStockAdjustment;

/// <summary>
/// Covers Opening Stock, Damaged Stock and Expired Stock adjustments (via AdjustmentType) as well as
/// free-form Manual corrections (where IsIncrease decides direction). Always creates a matching StockTransaction.
/// </summary>
public record CreateStockAdjustmentCommand(
    int ProductId,
    StockAdjustmentType AdjustmentType,
    double Quantity,
    bool IsIncrease,
    string? Reason) : IRequest<Result<int>>;
