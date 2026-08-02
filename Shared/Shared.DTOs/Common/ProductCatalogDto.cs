namespace Shared.DTOs.Common;

public class ProductCatalogDto
{
    public int Id { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public double PurchasePrice { get; set; }

    public double SellingPrice { get; set; }

    public double GSTPercentage { get; set; }

    public double CurrentStockQty { get; set; }

    public double MinStockQty { get; set; }
}
