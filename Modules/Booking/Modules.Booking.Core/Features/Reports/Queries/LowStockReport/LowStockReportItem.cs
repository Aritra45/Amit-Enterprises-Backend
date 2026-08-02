namespace Modules.Booking.Core.Features.Reports.Queries.LowStockReport;

public class LowStockReportItem
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public double CurrentStockQty { get; set; }

    public double MinStockQty { get; set; }

    public double ShortfallQty => Math.Max(0, MinStockQty - CurrentStockQty);
}
