using Microsoft.AspNetCore.Mvc;
using TemplateDapper.Api.Services;

namespace TemplateDapper.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSerilog(configuration);

        services.ConfigureSwaggerGenOptions();

        services.ConfigureAuthentication(configuration);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddScoped<ICachedUserService, CachedUserService>();

        return services;
    }
}
