using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Booking.Core.Extensions;
using Modules.Booking.Infrastructure.Extensions;

namespace Modules.Booking.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBookingCore();
        services.AddBookingInfrastructure(configuration);

        return services;
    }
}
