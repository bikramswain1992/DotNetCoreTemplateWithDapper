using Serilog;

namespace TemplateDapper.Api.Extensions;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(configuration)
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });
    }
}
