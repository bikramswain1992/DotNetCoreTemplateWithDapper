using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Services;

namespace TemplateDapper.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.ConfigureMapster();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
