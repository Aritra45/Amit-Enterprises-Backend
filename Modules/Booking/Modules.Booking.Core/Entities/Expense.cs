using Shared.Core.Entities;

namespace Modules.Booking.Core.Entities;

public class Expense : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Category { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }
}
