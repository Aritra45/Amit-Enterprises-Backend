namespace Modules.Booking.Core.Features.Reports.Queries.ProfitReport;

public class ProfitReportResponse
{
    public double TotalRevenue { get; set; }

    public double TotalCostOfGoodsSold { get; set; }

    public double GrossProfit { get; set; }

    public double TotalExpenses { get; set; }

    public double NetProfit { get; set; }
}
