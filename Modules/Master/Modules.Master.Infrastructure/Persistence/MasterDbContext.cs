using Microsoft.EntityFrameworkCore;
using Modules.Master.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Infrastructure.Persistence;

namespace Modules.Master.Infrastructure.Persistence;

public class MasterDbContext : AuditableDbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options, ICurrentUser? currentUser = null)
        : base(options, currentUser)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Settings> Settings => Set<Settings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Master");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDbContext).Assembly);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
