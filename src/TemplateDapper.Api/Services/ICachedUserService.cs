namespace TemplateDapper.Api.Services;

public interface ICachedUserService
{
    Task ClearCachedUsers(string key);
}