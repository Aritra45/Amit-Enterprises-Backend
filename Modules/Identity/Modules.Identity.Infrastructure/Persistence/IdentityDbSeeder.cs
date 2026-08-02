using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Entities;
using Shared.Core.Abstractions;
using Shared.Core.Constants;

namespace Modules.Identity.Infrastructure.Persistence;

public static class IdentityDbSeeder
{
    public static async Task SeedAsync(IdentityDbContext context, IPasswordHasher passwordHasher)
    {
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { Name = Roles.SuperAdmin, Description = "Full system access across all shops." },
                new Role { Name = Roles.ShopOwner, Description = "Manages a single shop's catalog, billing and stock." });

            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync())
        {
            var superAdminRole = await context.Roles.FirstAsync(r => r.Name == Roles.SuperAdmin);

            context.Users.Add(new User
            {
                FullName = "Super Admin",
                Email = "admin@amitenterprises.com",
                PasswordHash = passwordHasher.Hash("Admin@123"),
                RoleId = superAdminRole.Id
            });

            await context.SaveChangesAsync();
        }
    }
}
