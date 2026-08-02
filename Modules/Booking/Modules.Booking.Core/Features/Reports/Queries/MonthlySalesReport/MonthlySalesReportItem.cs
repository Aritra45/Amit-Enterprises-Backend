namespace Modules.Booking.Core.Features.Reports.Queries.MonthlySalesReport;

public class MonthlySalesReportItem
{
    public int Year { get; set; }

    public int Month { get; set; }

    public double TotalSales { get; set; }

    public int OrderCount { get; set; }
}
