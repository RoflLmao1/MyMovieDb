using MyMovieDb.Adapter.Tmdb.Movies;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyMovieDb.Adapter.Tmdb;

public class TmdbService : ITmdbService
{
    private static readonly JsonSerializerOptions _snakeCaseJsonSerializerOptions
        = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

    private readonly HttpClient _httpClient;

    public TmdbService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MovieDetailResult> GetMovieDetails(int id, CancellationToken cancellationToken = default)
    {
        using var stream = await _httpClient.GetStreamAsync($"/3/movie/{id}");

        return await JsonSerializer.DeserializeAsync<MovieDetailResult>(
            stream,
            options: _snakeCaseJsonSerializerOptions,
            cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize data from stream.");
    }

    public async Task<PagedAndDatedResult<MovieResult>> GetNowPlayingMovies([Range(1, int.MaxValue)] int page,
        CancellationToken cancellationToken = default)
    {
        string query = $"?page={page}";
        using var stream = await _httpClient.GetStreamAsync($"/3/movie/now_playing{query}");

        return await JsonSerializer.DeserializeAsync<PagedAndDatedResult<MovieResult>>(
            stream,
            options: _snakeCaseJsonSerializerOptions,
            cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize data from stream.");
    }
}
