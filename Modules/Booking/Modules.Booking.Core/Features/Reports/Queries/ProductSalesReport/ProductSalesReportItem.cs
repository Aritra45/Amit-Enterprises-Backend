namespace Modules.Booking.Core.Features.Reports.Queries.ProductSalesReport;

public class ProductSalesReportItem
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string ProductCode { get; set; } = string.Empty;

    public double QuantitySold { get; set; }

    public double Revenue { get; set; }
}
