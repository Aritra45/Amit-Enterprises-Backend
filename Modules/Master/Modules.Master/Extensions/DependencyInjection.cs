using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Master.Core.Extensions;
using Modules.Master.Infrastructure.Extensions;

namespace Modules.Master.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMasterModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMasterCore();
        services.AddMasterInfrastructure(configuration);

        return services;
    }
}
