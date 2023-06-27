using Microsoft.OpenApi.Models;

namespace TemplateDapper.Api.Extensions;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwaggerGenOptions(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Template",
                Version = "api-template-service-v1",
                Description = "Template services API",
                Contact = new OpenApiContact
                {
                    Name = "Template",
                    Url = new Uri("https://template.om")
                }
            });

            c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT authorization using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }, new List<string>() }
            });
        });

        return services;
    }
}
