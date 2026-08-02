namespace Modules.Master.Core.Features.Products;

public class ProductResponse
{
    public int Id { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double PurchasePrice { get; set; }

    public double SellingPrice { get; set; }

    public double GSTPercentage { get; set; }

    public double CurrentStockQty { get; set; }

    public double MinStockQty { get; set; }

    public string? ProductImage { get; set; }

    public bool IsLowStock => CurrentStockQty <= MinStockQty;

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }
}
