using TemplateDapper.Domain.ValueObjects;

namespace TemplateDapper.Application.Interfaces;

public interface ICurrentUserService
{
    CurrentUser CurrentUser { get; set; }
    Task<CurrentUser> GetCurrentUser(string token);
}
