using Microsoft.EntityFrameworkCore;
using Modules.Booking.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Infrastructure.Persistence;

namespace Modules.Booking.Infrastructure.Persistence;

public class BookingDbContext : AuditableDbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options, ICurrentUser? currentUser = null)
        : base(options, currentUser)
    {
    }

    public DbSet<Sale> Sales => Set<Sale>();

    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Booking");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
