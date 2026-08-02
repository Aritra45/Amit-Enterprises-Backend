using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Infrastructure.Persistence;
using Modules.Booking.Infrastructure.Persistence.Repositories;

namespace Modules.Booking.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql
                    .MigrationsAssembly(typeof(BookingDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();

        return services;
    }
}
