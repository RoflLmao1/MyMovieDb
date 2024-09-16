using Moq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace MyMovieDb.Users;

public class AuthorizeUserHandlerTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    private readonly Mock<IRepository<User, string>> _mockUserRepository = new Mock<IRepository<User, string>>();
    private readonly Mock<IUserAuthorizationService> _mockUserAuthorizationtService = new Mock<IUserAuthorizationService>();

    private AuthorizeUserHandler CreateHandler()
        => new AuthorizeUserHandler(
            _mockUserRepository.Object,
            _mockUserAuthorizationtService.Object)
        {
            LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>()
        };

    [Fact]
    public async Task Handle_NewUser_ShouldReturnResult()
    {
        // Arrange
        var userAuthorizationResult = new UserAuthorizationResult
        {
            AccessToken = Guid.NewGuid().ToString(),
            TokenType = "Bearer",
            ExpiresIn = 86400
        };
        _mockUserAuthorizationtService
            .Setup(x => x.Authorize(It.IsAny<UserAuthorization>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userAuthorizationResult);

        _mockUserRepository
            .Setup(x => x.AsyncExecuter)
            .Returns(GetRequiredService<IAsyncQueryableExecuter>());
        _mockUserRepository
            .Setup(x => x.GetQueryableAsync())
            .ReturnsAsync(new List<User>().AsQueryable());
        _mockUserRepository
            .Setup(x => x.InsertAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        var userAuthorization = new AuthorizeUserRequest
        {
            Audience = "test-audience",
            ClientId = Guid.NewGuid().ToString(),
            ClientSecret = Guid.NewGuid().ToString(),
            GrantType = "client_credentials"
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(userAuthorization, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userAuthorizationResult.AccessToken, result.AccessToken);
        Assert.Equal(userAuthorizationResult.ExpiresIn, result.ExpiresIn);
        Assert.Equal(userAuthorizationResult.TokenType, result.TokenType);

        _mockUserRepository.Verify(
            x => x.InsertAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingUser_ShouldReturnResult()
    {
        // Arrange
        var userAuthorizationResult = new UserAuthorizationResult
        {
            AccessToken = Guid.NewGuid().ToString(),
            TokenType = "Bearer",
            ExpiresIn = 86400
        };
        _mockUserAuthorizationtService
            .Setup(x => x.Authorize(It.IsAny<UserAuthorization>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userAuthorizationResult);

        string clientId = Guid.NewGuid().ToString();
        _mockUserRepository
            .Setup(x => x.AsyncExecuter)
            .Returns(GetRequiredService<IAsyncQueryableExecuter>());
        _mockUserRepository
            .Setup(x => x.GetQueryableAsync())
            .ReturnsAsync(new List<User>
            {
                new User(clientId)
            }.AsQueryable());
        _mockUserRepository
            .Setup(x => x.InsertAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        var userAuthorization = new AuthorizeUserRequest
        {
            Audience = "test-audience",
            ClientId = clientId,
            ClientSecret = Guid.NewGuid().ToString(),
            GrantType = "client_credentials"
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(userAuthorization, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userAuthorizationResult.AccessToken, result.AccessToken);
        Assert.Equal(userAuthorizationResult.ExpiresIn, result.ExpiresIn);
        Assert.Equal(userAuthorizationResult.TokenType, result.TokenType);

        _mockUserRepository.Verify(
            x => x.InsertAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
