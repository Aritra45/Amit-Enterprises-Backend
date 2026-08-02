using Shared.Core.Entities;

namespace Modules.Booking.Core.Entities;

public class Sale : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; } = DateTime.UtcNow;

    public double SubTotal { get; set; }

    public double DiscountAmount { get; set; }

    public double GSTAmount { get; set; }

    public double GrandTotal { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerMobile { get; set; }

    public string PaymentMode { get; set; } = "Cash";

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
