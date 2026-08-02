namespace Modules.Booking.Core.Features.Sales;

public class SaleItemResponse
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string ProductCode { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public double UnitPrice { get; set; }

    public double GSTPercentage { get; set; }

    public double DiscountAmount { get; set; }

    public double GSTAmount { get; set; }

    public double TotalAmount { get; set; }
}

public class SaleResponse
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; }

    public double SubTotal { get; set; }

    public double DiscountAmount { get; set; }

    public double GSTAmount { get; set; }

    public double GrandTotal { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerMobile { get; set; }

    public string PaymentMode { get; set; } = string.Empty;

    public List<SaleItemResponse> Items { get; set; } = new();
}
