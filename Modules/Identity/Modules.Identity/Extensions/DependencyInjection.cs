using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identity.Core.Extensions;
using Modules.Identity.Infrastructure.Extensions;
using Shared.Core.Settings;

namespace Modules.Identity.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore();
        services.AddIdentityInfrastructure(configuration);

        services.Configure<MasterAuthSettings>(configuration.GetSection("MasterAuth"));

        return services;
    }
}
