using Shared.Core.Entities;

namespace Modules.Booking.Core.Entities;

public class SaleItem : BaseEntity
{
    public int SaleId { get; set; }

    public Sale Sale { get; set; } = null!;

    public int ProductId { get; set; }

    /// <summary>Snapshot of the product name at sale time, so historical invoices stay accurate if the product is later renamed.</summary>
    public string ProductName { get; set; } = string.Empty;

    public string ProductCode { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public double UnitPrice { get; set; }

    /// <summary>Snapshot of the product's purchase (cost) price at sale time, used for profit reporting.</summary>
    public double PurchasePrice { get; set; }

    public double GSTPercentage { get; set; }

    public double DiscountAmount { get; set; }

    public double GSTAmount { get; set; }

    public double TotalAmount { get; set; }
}
