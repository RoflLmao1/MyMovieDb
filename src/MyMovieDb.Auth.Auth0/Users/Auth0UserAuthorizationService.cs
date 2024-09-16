using Microsoft.Extensions.Options;
using MyMovieDb.Adapter.Tmdb;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyMovieDb.Users;

public class Auth0UserAuthorizationService : IUserAuthorizationService
{
    private static readonly JsonSerializerOptions _snakeCaseJsonSerializerOptions
        = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

    private readonly HttpClient _httpClient;
    private readonly Auth0Options _auth0Options;

    public Auth0UserAuthorizationService(
        HttpClient httpClient,
        IOptions<Auth0Options> auth0Options)
    {
        _httpClient = httpClient;
        _auth0Options = auth0Options.Value;
    }

    public async Task<UserAuthorizationResult> Authorize(UserAuthorization userAuthorization,
        CancellationToken cancellationToken = default)
    {
        string json = JsonSerializer.Serialize(userAuthorization, options: _snakeCaseJsonSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpResponseMessage = await _httpClient.PostAsync(_auth0Options.TokenUri, content);
        httpResponseMessage.EnsureSuccessStatusCode();

        // Get content
        using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<UserAuthorizationResult>(
            stream,
            options: _snakeCaseJsonSerializerOptions,
            cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize data from stream.");
    }
}
