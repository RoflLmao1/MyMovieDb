using Moq;
using Moq.Protected;
using MyMovieDb.Adapter.Tmdb.Movies;
using System.Net;
using System.Text.Json;

namespace MyMovieDb.Adapter.Tmdb;

public class TmdbServiceTests : MyMovieDbAdapterTmdbTestBase<MyMovieDbAdapterTmdbTestModule>
{
    private static readonly JsonSerializerOptions _snakeCaseJsonSerializerOptions
        = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

    private TmdbService CreateService()
        => new TmdbService(
            new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.google.com")
            });

    [Fact]
    public async Task GetMovieDetails_ShouldReturnResult()
    {
        // Arrange
        var movieDetailResult = new MovieDetailResult
        {
            Id = 123
        };
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(movieDetailResult, options: _snakeCaseJsonSerializerOptions))
            });

        // Act
        var service = CreateService();
        var result = await service.GetMovieDetails(movieDetailResult.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(movieDetailResult.Id, result.Id);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task GetMovieDetails_NonSuccessStatusCode_ThrowException(HttpStatusCode httpStatusCode)
    {
        // Arrange
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(httpStatusCode));

        // Act
        var service = CreateService();
        var task = service.GetMovieDetails(123);

        // Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => task);
        Assert.Equal(httpStatusCode, exception.StatusCode);
    }

    [Fact]
    public async Task GetMovieDetails_EmptyStream_ShouldThrowException()
    {
        // Arrange
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            });

        // Act
        var service = CreateService();
        var task = service.GetMovieDetails(123);

        // Assert
        await Assert.ThrowsAsync<JsonException>(() => task);
    }

    [Fact]
    public async Task GetNowPlayingMovies_ShouldReturnResult()
    {
        // Arrange
        var movieResults = new PagedAndDatedResult<MovieResult>
        {
            Dates = new DateRange(),
            Page = 1,
            Results = new List<MovieResult>
            {
                new MovieResult
                {
                    Id = 123
                }
            },
            TotalPages = 1,
            TotalResults = 1
        };
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(movieResults, options: _snakeCaseJsonSerializerOptions))
            });

        // Act
        var service = CreateService();
        var result = await service.GetNowPlayingMovies(movieResults.Page);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(movieResults.Page, result.Page);
        Assert.Equal(movieResults.TotalPages, result.TotalPages);
        Assert.Equal(movieResults.TotalResults, result.TotalResults);
        Assert.Contains(
            movieResults.Results,
            x => result.Results.Any(y => y.Id == x.Id));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task GetNowPlayingMovies_NonSuccessStatusCode_ThrowException(HttpStatusCode httpStatusCode)
    {
        // Arrange
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(httpStatusCode));

        // Act
        var service = CreateService();
        var task = service.GetNowPlayingMovies(1);

        // Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => task);
        Assert.Equal(httpStatusCode, exception.StatusCode);
    }

    [Fact]
    public async Task GetNowPlayingMovies_EmptyStream_ShouldThrowException()
    {
        // Arrange
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            });

        // Act
        var service = CreateService();
        var task = service.GetNowPlayingMovies(1);

        // Assert
        await Assert.ThrowsAsync<JsonException>(() => task);
    }
}
