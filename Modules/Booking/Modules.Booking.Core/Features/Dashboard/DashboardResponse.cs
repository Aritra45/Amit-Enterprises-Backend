namespace Modules.Booking.Core.Features.Dashboard;

public class LowStockProductDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public double CurrentStockQty { get; set; }

    public double MinStockQty { get; set; }
}

public class TopSellingProductDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public double QuantitySold { get; set; }

    public double Revenue { get; set; }
}

public class RecentOrderDto
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; }

    public double GrandTotal { get; set; }
}

public class DashboardResponse
{
    public double TodaysSales { get; set; }

    public int TodaysOrders { get; set; }

    public double MonthlyRevenue { get; set; }

    public int TotalProducts { get; set; }

    public double TotalExpenses { get; set; }

    public List<LowStockProductDto> LowStockProducts { get; set; } = new();

    public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();

    public List<RecentOrderDto> RecentOrders { get; set; } = new();
}
