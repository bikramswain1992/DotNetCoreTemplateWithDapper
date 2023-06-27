using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateDapper.Application.Extensions;

public static class MapsterConfiguration
{
    public static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings
            .Default
            .IgnoreNullValues(true);


        return services;
    }
}
