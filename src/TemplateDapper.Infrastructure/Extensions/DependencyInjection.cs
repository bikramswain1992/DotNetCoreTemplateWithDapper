using Microsoft.Extensions.DependencyInjection;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Infrastructure.Services;

namespace TemplateDapper.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services)
    {
        services.AddSingleton<IIdentityService, IdentityService>();

        return services;
    }
}
