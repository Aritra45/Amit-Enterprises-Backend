using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Identity.Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityCore(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(cfg => { }, assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
