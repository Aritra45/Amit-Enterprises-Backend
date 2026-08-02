namespace Modules.Booking.Core.Abstractions;

public record TopSellingProductProjection(int ProductId, string ProductName, double QuantitySold, double Revenue);

public record ProductSalesProjection(int ProductId, string ProductName, string ProductCode, double QuantitySold, double Revenue);

public record DailySalesProjection(DateTime Date, double TotalSales, int OrderCount);

public record MonthlySalesProjection(int Year, int Month, double TotalSales, int OrderCount);
