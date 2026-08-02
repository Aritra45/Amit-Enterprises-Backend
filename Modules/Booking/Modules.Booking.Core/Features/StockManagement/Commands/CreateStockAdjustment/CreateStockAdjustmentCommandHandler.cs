using System.Transactions;
using MediatR;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.StockManagement.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentCommandHandler : IRequestHandler<CreateStockAdjustmentCommand, Result<int>>
{
    private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
    private readonly IStockTransactionRepository _stockTransactionRepository;
    private readonly IProductCatalogService _productCatalogService;

    public CreateStockAdjustmentCommandHandler(
        IStockAdjustmentRepository stockAdjustmentRepository,
        IStockTransactionRepository stockTransactionRepository,
        IProductCatalogService productCatalogService)
    {
        _stockAdjustmentRepository = stockAdjustmentRepository;
        _stockTransactionRepository = stockTransactionRepository;
        _productCatalogService = productCatalogService;
    }

    public async Task<Result<int>> Handle(CreateStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        using var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        var product = await _productCatalogService.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Product", request.ProductId);

        var (transactionType, isIncrease) = request.AdjustmentType switch
        {
            StockAdjustmentType.OpeningStock => (StockTransactionType.OpeningStock, true),
            StockAdjustmentType.Damaged => (StockTransactionType.Damaged, false),
            StockAdjustmentType.Expired => (StockTransactionType.Expired, false),
            StockAdjustmentType.Manual => (StockTransactionType.Adjustment, request.IsIncrease),
            _ => throw new ValidationException("Unsupported adjustment type.")
        };

        var signedChange = isIncrease ? request.Quantity : -request.Quantity;

        if (signedChange < 0 && product.CurrentStockQty + signedChange < 0)
        {
            throw new ConflictException(
                $"Insufficient stock for '{product.ProductName}'. Available: {product.CurrentStockQty}, requested: {request.Quantity}.");
        }

        var adjustment = new StockAdjustment
        {
            ProductId = product.Id,
            ProductName = product.ProductName,
            AdjustmentType = request.AdjustmentType,
            Quantity = request.Quantity,
            IsIncrease = isIncrease,
            Reason = request.Reason,
            AdjustmentDate = DateTime.UtcNow
        };

        await _stockAdjustmentRepository.AddAsync(adjustment, cancellationToken);
        await _stockAdjustmentRepository.SaveChangesAsync(cancellationToken);

        await _productCatalogService.AdjustStockAsync(product.Id, signedChange, cancellationToken);

        await _stockTransactionRepository.AddAsync(new StockTransaction
        {
            ProductId = product.Id,
            ProductName = product.ProductName,
            TransactionType = transactionType,
            QuantityChange = signedChange,
            StockAfterTransaction = product.CurrentStockQty + signedChange,
            ReferenceNumber = $"ADJ-{adjustment.Id}",
            Remarks = request.Reason
        }, cancellationToken);
        await _stockTransactionRepository.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return Result<int>.Success(adjustment.Id, "Stock adjustment recorded successfully.");
    }
}
