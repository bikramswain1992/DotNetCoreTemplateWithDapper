using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using TemplateDapper.Api.Extensions;
using TemplateDapper.Api.Middlewares;
using TemplateDapper.Application.Extensions;
using TemplateDapper.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Host.UseSerilog();

builder.Services.AddApiDependency(config);
builder.Services.AddInfrastructureDependency();
builder.Services.AddApplicationDependency();

builder.Services.AddMemoryCache();

builder.Services.AddControllers()
    .ConfigureSerialization();

var app = builder.Build();

app.UseHsts();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TemplateDapperServices"));

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.UseMiddleware<IdentityServiceMiddleware>();

app.MapControllers();

app.Run();
