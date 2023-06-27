using TemplateDapper.Application.Interfaces;
using TemplateDapper.Domain.ValueObjects;

namespace TemplateDapper.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IIdentityService _identityService;

    public CurrentUser CurrentUser { get; set; } = default!;

    public CurrentUserService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<CurrentUser> GetCurrentUser(string token)
    {
        if (CurrentUser == null)
        {
            CurrentUser = await _identityService.GetCurrentUser(token);
        }

        return CurrentUser;
    }
}
