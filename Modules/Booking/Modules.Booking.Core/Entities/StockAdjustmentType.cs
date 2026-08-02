namespace Modules.Booking.Core.Entities;

public enum StockAdjustmentType
{
    /// <summary>Initial stock entry for a product. Increases stock.</summary>
    OpeningStock = 1,

    /// <summary>Stock written off as damaged. Decreases stock.</summary>
    Damaged = 2,

    /// <summary>Stock written off as expired. Decreases stock.</summary>
    Expired = 3,

    /// <summary>Manual correction. Sign of Quantity determines direction.</summary>
    Manual = 4
}
