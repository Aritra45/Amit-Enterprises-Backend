namespace Modules.Booking.Core.Features.Reports.Queries.ExpenseReport;

public class ExpenseCategoryBreakdown
{
    public string Category { get; set; } = string.Empty;

    public double TotalAmount { get; set; }
}

public class ExpenseReportResponse
{
    public double TotalExpenses { get; set; }

    public List<ExpenseCategoryBreakdown> ByCategory { get; set; } = new();
}
