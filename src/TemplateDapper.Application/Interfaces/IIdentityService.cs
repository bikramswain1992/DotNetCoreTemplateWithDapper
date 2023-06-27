using TemplateDapper.Domain.ValueObjects;

namespace TemplateDapper.Application.Interfaces;

public interface IIdentityService
{
    Task<string> GenerateToken(CurrentUser request);
    Task<CurrentUser> GetCurrentUser(string token);
}
