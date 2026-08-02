using Shared.Core.Entities;

namespace Modules.Booking.Core.Entities;

/// <summary>Immutable ledger entry recording every stock movement, regardless of the cause.</summary>
public class StockTransaction : BaseEntity
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public StockTransactionType TransactionType { get; set; }

    /// <summary>Signed change applied to stock (positive = added, negative = deducted).</summary>
    public double QuantityChange { get; set; }

    public double StockAfterTransaction { get; set; }

    /// <summary>Invoice number, stock adjustment id, etc. - whatever triggered this transaction.</summary>
    public string? ReferenceNumber { get; set; }

    public string? Remarks { get; set; }
}
