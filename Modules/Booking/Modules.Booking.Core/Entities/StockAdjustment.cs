using Shared.Core.Entities;

namespace Modules.Booking.Core.Entities;

/// <summary>Records the request that caused a stock movement (opening stock, damaged, expired, manual correction).</summary>
public class StockAdjustment : BaseEntity
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public StockAdjustmentType AdjustmentType { get; set; }

    /// <summary>
    /// Magnitude entered by the user (always &gt;= 0). The resulting stock movement direction is derived from
    /// AdjustmentType: OpeningStock adds, Damaged/Expired subtract, Manual uses <see cref="IsIncrease"/> to decide.
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>Only meaningful for AdjustmentType.Manual - whether Quantity should be added to or subtracted from stock.</summary>
    public bool IsIncrease { get; set; }

    public string? Reason { get; set; }

    public DateTime AdjustmentDate { get; set; } = DateTime.UtcNow;
}
