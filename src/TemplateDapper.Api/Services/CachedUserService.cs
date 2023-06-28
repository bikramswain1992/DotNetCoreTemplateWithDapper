using MediatR;
using Microsoft.Extensions.Caching.Memory;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Api.Services;

public class CachedUserService : ICachedUserService
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _memoryCache;

    public CachedUserService(
        IMediator mediator,
        IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    public async Task ClearCachedUsers(string key)
    {
        await Task.Run(() => _memoryCache.Remove(key));
    }

    public async Task<CommandResponse<UserDto?>> GetCachedSurveyByLink(string link, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(link, async entry =>
        {
            var cacheOption = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            return await _mediator.Send(new GetUserByIdModel(new Guid(entry.Key.ToString()!)), cancellationToken);
        });
    }
}
