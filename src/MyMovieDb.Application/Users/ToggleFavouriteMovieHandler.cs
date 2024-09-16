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
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;

namespace MyMovieDb.Users;

public class ToggleFavouriteMovieHandler : ApplicationService,
    IRequestHandler<ToggleFavouriteMovieRequest, bool>
{
    public static readonly string CACHE_KEY_PREFIX = $"{nameof(ToggleFavouriteMovieHandler)}_Id_";
    private static readonly DistributedCacheEntryOptions _distributedCacheEntryOptions
        = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(10),
        };

    private readonly IRepository<UserFavourite> _userFavouriteRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly ITmdbService _tmdbService;
    private readonly IDistributedCache<MovieDetailResult> _distributedCache;

    public ToggleFavouriteMovieHandler(
        IRepository<UserFavourite> repository,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        ITmdbService tmdbService,
        IDistributedCache<MovieDetailResult> distributedCache)
    {
        _userFavouriteRepository = repository;
        _currentPrincipalAccessor = currentPrincipalAccessor;
        _tmdbService = tmdbService;
        _distributedCache = distributedCache;
    }

    public async Task<bool> Handle(ToggleFavouriteMovieRequest request, CancellationToken cancellationToken)
    {
        // Get client Id
        string? clientId = _currentPrincipalAccessor.Principal.GetClientId()
            ?? throw new UnauthorizedAccessException();

        // Retrieve user's favourite movie
        var userFavourite = await _userFavouriteRepository.FindAsync(
            x => x.UserId == clientId && x.MovieId == request.MovieId,
            includeDetails: false,
            cancellationToken: cancellationToken);

        // The movie is not a favourite yet AND user is marking as favourite
        if (userFavourite == null && request.MarkAsFavourite)
        {
            // Get details
            string cacheKey = $"{CACHE_KEY_PREFIX}{request.MovieId}";

            var movie = (await _distributedCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    try
                    {
                        return await _tmdbService.GetMovieDetails(
                            request.MovieId,
                            cancellationToken: cancellationToken);
                    }
                    catch (HttpRequestException ex)
                    when ((HttpStatusCode)ex.GetType().GetProperty("StatusCode").GetValue(ex) == HttpStatusCode.NotFound)
                    {
                        throw new BusinessException(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Code)
                            .WithData(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Formats.Id, request.MovieId);
                    }
                },
                optionsFactory: () => _distributedCacheEntryOptions,
                token: cancellationToken))!;

            // Create new favourite movie
            userFavourite = ObjectMapper.Map(
                movie,
                new UserFavourite
                {
                    UserId = clientId,
                    MovieId = request.MovieId,
                });

            // Add as favourite
            await _userFavouriteRepository.InsertAsync(
                userFavourite,
                cancellationToken: cancellationToken);
        }
        // The movie is a favourite AND user is removing as favourite
        else if (userFavourite != null && !request.MarkAsFavourite)
        {
            // Remove from favourite
            await _userFavouriteRepository.DeleteAsync(
                userFavourite,
                cancellationToken: cancellationToken);
        }

        return true;
    }
}

public class ToggleFavouriteMovieRequest : IRequest<bool>
{
    /// <summary>
    /// Movie Id
    /// </summary>
    public int MovieId { get; set; }
    /// <summary>
    /// Determines whether to mark the movie as favourite
    /// </summary>
    public bool MarkAsFavourite { get; set; }
}
