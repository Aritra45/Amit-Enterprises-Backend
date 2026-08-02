using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Infrastructure.Persistence;

namespace Modules.Identity.Infrastructure.Persistence;

public class IdentityDbContext : AuditableDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ICurrentUser? currentUser = null)
        : base(options, currentUser)
    {
    }

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Identity");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
