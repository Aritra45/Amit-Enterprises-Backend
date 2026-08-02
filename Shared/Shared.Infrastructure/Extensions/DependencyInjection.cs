using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Core.Abstractions;
using Shared.Core.Settings;
using Shared.Infrastructure.Identity;
using Shared.Infrastructure.Storage;

namespace Shared.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            return new Cloudinary(account) { Api = { Secure = true } };
        });
        services.AddSingleton<IFileStorageService, CloudinaryFileStorageService>();

        return services;
    }
}
