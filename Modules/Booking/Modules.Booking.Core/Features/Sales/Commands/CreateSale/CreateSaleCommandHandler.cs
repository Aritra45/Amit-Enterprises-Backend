using System.Transactions;
using MediatR;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IStockTransactionRepository _stockTransactionRepository;
    private readonly IProductCatalogService _productCatalogService;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        IStockTransactionRepository stockTransactionRepository,
        IProductCatalogService productCatalogService)
    {
        _saleRepository = saleRepository;
        _stockTransactionRepository = stockTransactionRepository;
        _productCatalogService = productCatalogService;
    }

    public async Task<Result<SaleResponse>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Sale/SaleItem/StockTransaction live in BookingDbContext; the Product stock update lives in
        // MasterDbContext. Both point at the same physical database, so an ambient TransactionScope
        // keeps the whole operation atomic across the two DbContexts.
        using var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        var invoiceNumber = await _saleRepository.GenerateNextInvoiceNumberAsync(cancellationToken);

        var sale = new Sale
        {
            InvoiceNumber = invoiceNumber,
            SaleDate = DateTime.UtcNow,
            CustomerName = request.CustomerName,
            CustomerMobile = request.CustomerMobile,
            PaymentMode = request.PaymentMode,
            DiscountAmount = request.DiscountAmount
        };

        var stockMovements = new List<(int ProductId, string ProductName, double QuantitySold, double StockAfter)>();

        double subTotal = 0;
        double gstTotal = 0;

        foreach (var itemRequest in request.Items)
        {
            var product = await _productCatalogService.GetByIdAsync(itemRequest.ProductId, cancellationToken)
                ?? throw new NotFoundException("Product", itemRequest.ProductId);

            if (product.CurrentStockQty < itemRequest.Quantity)
            {
                throw new ConflictException(
                    $"Insufficient stock for '{product.ProductName}'. Available: {product.CurrentStockQty}, requested: {itemRequest.Quantity}.");
            }

            var lineSubTotal = product.SellingPrice * itemRequest.Quantity;
            var lineTaxable = lineSubTotal - itemRequest.DiscountAmount;
            var lineGst = lineTaxable * product.GSTPercentage / 100;
            var lineTotal = lineTaxable + lineGst;

            sale.SaleItems.Add(new SaleItem
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                Quantity = itemRequest.Quantity,
                UnitPrice = product.SellingPrice,
                PurchasePrice = product.PurchasePrice,
                GSTPercentage = product.GSTPercentage,
                DiscountAmount = itemRequest.DiscountAmount,
                GSTAmount = lineGst,
                TotalAmount = lineTotal
            });

            stockMovements.Add((product.Id, product.ProductName, itemRequest.Quantity, product.CurrentStockQty - itemRequest.Quantity));

            subTotal += lineSubTotal;
            gstTotal += lineGst;
        }

        sale.SubTotal = subTotal;
        sale.GSTAmount = gstTotal;
        sale.GrandTotal = subTotal + gstTotal - request.DiscountAmount;

        await _saleRepository.AddAsync(sale, cancellationToken);
        await _saleRepository.SaveChangesAsync(cancellationToken);

        foreach (var movement in stockMovements)
        {
            await _productCatalogService.AdjustStockAsync(movement.ProductId, -movement.QuantitySold, cancellationToken);

            await _stockTransactionRepository.AddAsync(new StockTransaction
            {
                ProductId = movement.ProductId,
                ProductName = movement.ProductName,
                TransactionType = StockTransactionType.Sale,
                QuantityChange = -movement.QuantitySold,
                StockAfterTransaction = movement.StockAfter,
                ReferenceNumber = sale.InvoiceNumber
            }, cancellationToken);
        }

        await _stockTransactionRepository.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        var response = new SaleResponse
        {
            Id = sale.Id,
            InvoiceNumber = sale.InvoiceNumber,
            SaleDate = sale.SaleDate,
            SubTotal = sale.SubTotal,
            DiscountAmount = sale.DiscountAmount,
            GSTAmount = sale.GSTAmount,
            GrandTotal = sale.GrandTotal,
            CustomerName = sale.CustomerName,
            CustomerMobile = sale.CustomerMobile,
            PaymentMode = sale.PaymentMode,
            Items = sale.SaleItems.Select(i => new SaleItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                GSTPercentage = i.GSTPercentage,
                DiscountAmount = i.DiscountAmount,
                GSTAmount = i.GSTAmount,
                TotalAmount = i.TotalAmount
            }).ToList()
        };

        return Result<SaleResponse>.Success(response, "Sale completed successfully.");
    }
}
