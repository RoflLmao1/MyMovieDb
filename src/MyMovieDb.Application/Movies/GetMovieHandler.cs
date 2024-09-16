using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Adapter.Tmdb.Movies;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace MyMovieDb.Movies;

public class GetMovieHandler : ApplicationService,
    IRequestHandler<GetMovieRequest, MovieDetailDto>
{
    public static readonly string CACHE_KEY_PREFIX = $"{nameof(GetMoviesHandler)}_Id_";
    private static readonly DistributedCacheEntryOptions _distributedCacheEntryOptions
        = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(10),
        };

    private readonly ITmdbService _tmdbService;
    private readonly IDistributedCache<MovieDetailDto> _distributedCache;

    public GetMovieHandler(
        ITmdbService tmdbService,
        IDistributedCache<MovieDetailDto> distributedCache)
    {
        _tmdbService = tmdbService;
        _distributedCache = distributedCache;
    }

    public Task<MovieDetailDto> Handle(GetMovieRequest request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CACHE_KEY_PREFIX}{request.Id}";

        return _distributedCache.GetOrAddAsync(
            cacheKey,
            async () =>
            {
                try
                {
                    var movieDetail = await _tmdbService.GetMovieDetails(
                        request.Id,
                        cancellationToken: cancellationToken);

                    return ObjectMapper.Map<MovieDetailResult, MovieDetailDto>(movieDetail);
                }
                catch (HttpRequestException ex)
                when ((HttpStatusCode)ex.GetType().GetProperty("StatusCode").GetValue(ex) == HttpStatusCode.NotFound)
                {
                    throw new BusinessException(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Code)
                        .WithData(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Formats.Id, request.Id);
                }
            },
            optionsFactory: () => _distributedCacheEntryOptions,
            token: cancellationToken)!;
    }
}

public class GetMovieRequest : IRequest<MovieDetailDto>
{
    /// <summary>
    /// Movie Id
    /// </summary>
    public int Id { get; set; }

    public GetMovieRequest(int id)
    {
        Id = id;
    }
}
