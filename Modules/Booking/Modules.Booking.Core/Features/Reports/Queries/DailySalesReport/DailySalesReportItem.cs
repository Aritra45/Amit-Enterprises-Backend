namespace Modules.Booking.Core.Features.Reports.Queries.DailySalesReport;

public class DailySalesReportItem
{
    public DateTime Date { get; set; }

    public double TotalSales { get; set; }

    public int OrderCount { get; set; }
}
