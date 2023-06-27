using TemplateDapper.Application.Interfaces;

namespace TemplateDapper.Api.Middlewares;

public class IdentityServiceMiddleware
{
    private readonly RequestDelegate _next;
    private ICurrentUserService _currentUserService = default!;

    public IdentityServiceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var authToken = context.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.ToString();
        if (!string.IsNullOrEmpty(authToken))
        {
            _currentUserService = context.RequestServices.GetService<ICurrentUserService>()!;
            await _currentUserService.GetCurrentUser(authToken);
        }

        await _next(context);
    }
}
