using MyMovieDb.Adapter.Tmdb.Movies;
using System.Threading;
using System.Threading.Tasks;

namespace MyMovieDb.Adapter.Tmdb;

public interface ITmdbService
{
    /// <summary>
    /// Retrieves the details for a single movie.
    /// </summary>
    /// <param name="id">Movie id</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> object.</param>
    /// <returns>The movie details.</returns>
    Task<MovieDetailResult> GetMovieDetails(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves movies that are playing now.
    /// </summary>
    /// <param name="page">Page number fo retrieve</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> object.</param>
    /// <returns>A paged result of movies that are playing now.</returns>
    Task<PagedAndDatedResult<MovieResult>> GetNowPlayingMovies(int page, CancellationToken cancellationToken = default);
}
