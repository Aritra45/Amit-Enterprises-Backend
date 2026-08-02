using Modules.Booking.Core.Entities;

namespace Modules.Booking.Core.Features.StockManagement;

public class StockAdjustmentResponse
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public StockAdjustmentType AdjustmentType { get; set; }

    public double Quantity { get; set; }

    public bool IsIncrease { get; set; }

    public string? Reason { get; set; }

    public DateTime AdjustmentDate { get; set; }
}

public class StockTransactionResponse
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public StockTransactionType TransactionType { get; set; }

    public double QuantityChange { get; set; }

    public double StockAfterTransaction { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedOn { get; set; }
}
