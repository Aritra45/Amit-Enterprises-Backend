using Shared.Core.Entities;

namespace Modules.Master.Core.Entities;

public class Product : BaseEntity
{
    public string ProductCode { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public double PurchasePrice { get; set; }

    public double SellingPrice { get; set; }

    public double GSTPercentage { get; set; }

    public double CurrentStockQty { get; set; }

    public double MinStockQty { get; set; }

    public string? ProductImage { get; set; }
}
