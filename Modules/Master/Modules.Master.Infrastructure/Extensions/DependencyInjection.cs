using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Master.Core.Abstractions;
using Modules.Master.Infrastructure.Persistence;
using Modules.Master.Infrastructure.Persistence.Repositories;
using Modules.Master.Infrastructure.Services;
using Shared.Core.Abstractions;

namespace Modules.Master.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMasterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MasterDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql
                    .MigrationsAssembly(typeof(MasterDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISettingsRepository, SettingsRepository>();

        services.AddScoped<IProductCatalogService, ProductCatalogService>();

        return services;
    }
}
