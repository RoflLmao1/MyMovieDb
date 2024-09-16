using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Users.Specifications;
using System.Net;
using System.Text.Json;

namespace MyMovieDb.Users;

public class Auth0UserAuthorizationServiceTests : MyMovieDbAuthAuth0TestBase<MyMovieDbAuthAuth0TestModule>
{
    private static readonly JsonSerializerOptions _snakeCaseJsonSerializerOptions
        = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    private readonly Mock<IOptions<Auth0Options>> _mockAuth0Options = new Mock<IOptions<Auth0Options>>();

    private Auth0UserAuthorizationService CreateService()
        => new Auth0UserAuthorizationService(
            new HttpClient(_mockHttpMessageHandler.Object),
            _mockAuth0Options.Object);

    [Fact]
    public async Task Authorize_ShouldReturnResult()
    {
        // Arrange
        var authOptions = new Auth0Options
        {
            TokenUri = "https://www.google.com"
        };
        _mockAuth0Options
            .Setup(x => x.Value)
            .Returns(authOptions);

        var expectedUserAuthorizationResult = new UserAuthorizationResult
        {
            AccessToken = Guid.NewGuid().ToString(),
            TokenType = "Bearer",
            ExpiresIn = 86400
        };
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(expectedUserAuthorizationResult, options: _snakeCaseJsonSerializerOptions))
            });

        var userAuthorization = new UserAuthorization
        {
            Audience = "test-audience",
            ClientId = Guid.NewGuid().ToString(),
            ClientSecret = Guid.NewGuid().ToString(),
            GrantType = "client_credentials"
        };

        // Act
        var service = CreateService();
        var result = await service.Authorize(userAuthorization);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUserAuthorizationResult.AccessToken, result.AccessToken);
        Assert.Equal(expectedUserAuthorizationResult.ExpiresIn, result.ExpiresIn);
        Assert.Equal(expectedUserAuthorizationResult.TokenType, result.TokenType);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task Authorize_NonSuccessStatusCode_ThrowException(HttpStatusCode httpStatusCode)
    {
        // Arrange
        var authOptions = new Auth0Options
        {
            TokenUri = "https://www.google.com"
        };
        _mockAuth0Options
            .Setup(x => x.Value)
            .Returns(authOptions);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(httpStatusCode));

        var userAuthorization = new UserAuthorization
        {
            Audience = "test-audience",
            ClientId = Guid.NewGuid().ToString(),
            ClientSecret = Guid.NewGuid().ToString(),
            GrantType = "client_credentials"
        };

        // Act
        var service = CreateService();
        var task = service.Authorize(userAuthorization);

        // Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => task);
        Assert.Equal(httpStatusCode, exception.StatusCode);
    }

    [Fact]
    public async Task Authorize_EmptyStream_ShouldThrowException()
    {
        // Arrange
        var authOptions = new Auth0Options
        {
            TokenUri = "https://www.google.com"
        };
        _mockAuth0Options
            .Setup(x => x.Value)
            .Returns(authOptions);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            });

        var userAuthorization = new UserAuthorization
        {
            Audience = "test-audience",
            ClientId = Guid.NewGuid().ToString(),
            ClientSecret = Guid.NewGuid().ToString(),
            GrantType = "client_credentials"
        };

        // Act
        var service = CreateService();
        var task = service.Authorize(userAuthorization);

        // Assert
        await Assert.ThrowsAsync<JsonException>(() => task);
    }
}
