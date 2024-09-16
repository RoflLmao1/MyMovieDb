using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Adapter.Tmdb.Movies;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace MyMovieDb.Movies;

public class GetMoviesHandler : ApplicationService,
    IRequestHandler<GetMoviesRequest, PagedResultDto<MovieDto>>
{
    public static readonly string CACHE_KEY_PREFIX = $"{nameof(GetMoviesHandler)}_";
    private static readonly DistributedCacheEntryOptions _distributedCacheEntryOptions
        = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(10),
        };

    private readonly ITmdbService _tmdbService;
    private readonly IDistributedCache<PagedResultDto<MovieDto>> _distributedCache;

    public GetMoviesHandler(
        ITmdbService tmdbService,
        IDistributedCache<PagedResultDto<MovieDto>> distributedCache)
    {
        _tmdbService = tmdbService;
        _distributedCache = distributedCache;
    }

    public Task<PagedResultDto<MovieDto>> Handle(GetMoviesRequest request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CACHE_KEY_PREFIX}p{request.Page}";

        return _distributedCache.GetOrAddAsync(
            cacheKey,
            async () =>
            {
                var result = await _tmdbService.GetNowPlayingMovies(
                    request.Page,
                    cancellationToken: cancellationToken);

                return new PagedResultDto<MovieDto>
                {
                    Items = ObjectMapper.Map<IEnumerable<MovieResult>, IReadOnlyList<MovieDto>>(result.Results),
                    TotalCount = result.TotalResults
                };
            },
            optionsFactory: () => _distributedCacheEntryOptions,
            token: cancellationToken)!;
    }
}

public class GetMoviesRequest : IRequest<PagedResultDto<MovieDto>>
{
    public int Page { get; set; } = 1;
}
