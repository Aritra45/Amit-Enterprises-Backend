namespace Modules.Booking.Core.Features.Expenses;

public class ExpenseResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Category { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedOn { get; set; }
}
