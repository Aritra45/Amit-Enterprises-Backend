using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Helpers;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Dashboard;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, Result<DashboardResponse>>
{
    private const int TopSellingProductsCount = 5;
    private const int LowStockProductsCount = 10;
    private const int RecentOrdersCount = 10;

    private readonly ISaleRepository _saleRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IProductCatalogService _productCatalogService;

    public GetDashboardQueryHandler(
        ISaleRepository saleRepository,
        IExpenseRepository expenseRepository,
        IProductCatalogService productCatalogService)
    {
        _saleRepository = saleRepository;
        _expenseRepository = expenseRepository;
        _productCatalogService = productCatalogService;
    }

    public async Task<Result<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var todayStart = IstDateTime.TodayStartUtc();
        var todayEnd = todayStart.AddDays(1);
        var monthStart = IstDateTime.MonthStartUtc();

        var todaysSales = await _saleRepository.GetTotalSalesAsync(todayStart, todayEnd, cancellationToken);
        var todaysOrders = await _saleRepository.GetOrdersCountAsync(todayStart, todayEnd, cancellationToken);
        var monthlyRevenue = await _saleRepository.GetTotalSalesAsync(monthStart, todayEnd, cancellationToken);
        var totalProducts = await _productCatalogService.GetTotalProductsCountAsync(cancellationToken);
        var totalExpenses = await _expenseRepository.GetTotalExpensesAsync(monthStart, todayEnd, cancellationToken);
        var lowStockProducts = await _productCatalogService.GetLowStockProductsAsync(LowStockProductsCount, cancellationToken);
        var topSellingProducts = await _saleRepository.GetTopSellingProductsAsync(TopSellingProductsCount, monthStart, todayEnd, cancellationToken);
        var recentOrders = await _saleRepository.GetRecentSalesAsync(RecentOrdersCount, cancellationToken);

        var response = new DashboardResponse
        {
            TodaysSales = todaysSales,
            TodaysOrders = todaysOrders,
            MonthlyRevenue = monthlyRevenue,
            TotalProducts = totalProducts,
            TotalExpenses = totalExpenses,
            LowStockProducts = lowStockProducts.Select(p => new LowStockProductDto
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                CurrentStockQty = p.CurrentStockQty,
                MinStockQty = p.MinStockQty
            }).ToList(),
            TopSellingProducts = topSellingProducts.Select(p => new TopSellingProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                QuantitySold = p.QuantitySold,
                Revenue = p.Revenue
            }).ToList(),
            RecentOrders = recentOrders.Select(s => new RecentOrderDto
            {
                Id = s.Id,
                InvoiceNumber = s.InvoiceNumber,
                SaleDate = s.SaleDate,
                GrandTotal = s.GrandTotal
            }).ToList()
        };

        return Result<DashboardResponse>.Success(response);
    }
}
