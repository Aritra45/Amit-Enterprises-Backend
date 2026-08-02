using Microsoft.EntityFrameworkCore;
using Shared.Core.Abstractions;
using Shared.Core.Entities;

namespace Shared.Infrastructure.Persistence;

public abstract class AuditableDbContext : DbContext
{
    private readonly ICurrentUser? _currentUser;

    protected AuditableDbContext(DbContextOptions options, ICurrentUser? currentUser = null)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    private void ApplyAuditInfo()
    {
        var userName = _currentUser?.UserName ?? "system";
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = now;
                    entry.Entity.CreatedBy = userName;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedOn = now;
                    entry.Entity.UpdatedBy = userName;
                    break;
            }
        }
    }
}
