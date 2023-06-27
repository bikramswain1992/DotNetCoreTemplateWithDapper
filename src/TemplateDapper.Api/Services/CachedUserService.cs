using MediatR;
using Microsoft.Extensions.Caching.Memory;

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

    //public async Task<CommandResponse<SurveyDto>> GetCachedSurveyByLink(string link, CancellationToken cancellationToken)
    //{
    //    return await _memoryCache.GetOrCreateAsync(link, async entry =>
    //    {
    //        var cacheOption = new MemoryCacheEntryOptions()
    //            .SetSlidingExpiration(TimeSpan.FromMinutes(3));
    //        return await _mediator.Send(new GetSurveyByLinkRequest(entry.Key.ToString()!), cancellationToken);
    //    });
    //}
}
