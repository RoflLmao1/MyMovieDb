using Moq;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Adapter.Tmdb.Movies;
using System.Linq.Expressions;
using System.Security.Claims;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;

namespace MyMovieDb.Users;

public class ToggleFavouriteMovieHandlerTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    private readonly Mock<IRepository<UserFavourite>> _mockUserFavouriteRepository = new Mock<IRepository<UserFavourite>>();
    private readonly Mock<ICurrentPrincipalAccessor> _mockCurrentPrincipalAccessor = new Mock<ICurrentPrincipalAccessor>();
    private readonly Mock<ITmdbService> _mockTmdbService = new Mock<ITmdbService>();

    private ToggleFavouriteMovieHandler CreateHandler()
        => new ToggleFavouriteMovieHandler(
            _mockUserFavouriteRepository.Object,
            _mockCurrentPrincipalAccessor.Object,
            _mockTmdbService.Object,
            GetRequiredService<IDistributedCache<MovieDetailResult>>())
        {
            LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>()
        };

    [Fact]
    public async Task Handle_IsNotFavouriteMovie_MarkingAsFavourite_ShouldMarkAsFavourite()
    {
        // Arrange
        string clientId = Guid.NewGuid().ToString();
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, clientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));
        _mockCurrentPrincipalAccessor
            .Setup(x => x.Principal)
            .Returns(claimsPrincipal);

        _mockUserFavouriteRepository
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserFavourite?)null);
        _mockUserFavouriteRepository
            .Setup(x => x.InsertAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovieDetailResult());

        var request = new ToggleFavouriteMovieRequest
        {
            MarkAsFavourite = true,
            MovieId = 123
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);

        _mockUserFavouriteRepository.Verify(
            x => x.InsertAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockTmdbService.Verify(
            x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_IsFavouriteMovie_MarkingAsFavourite_ShouldSkipMarkAsFavourite()
    {
        // Arrange
        string clientId = Guid.NewGuid().ToString();
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, clientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));
        _mockCurrentPrincipalAccessor
            .Setup(x => x.Principal)
            .Returns(claimsPrincipal);

        var userFavourite =
            new UserFavourite
            {
                UserId = clientId,
                MovieId = 456,
            };
        _mockUserFavouriteRepository
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userFavourite);
        _mockUserFavouriteRepository
            .Setup(x => x.InsertAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovieDetailResult());

        var request = new ToggleFavouriteMovieRequest
        {
            MarkAsFavourite = true,
            MovieId = userFavourite.MovieId
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);

        _mockUserFavouriteRepository.Verify(
            x => x.InsertAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockTmdbService.Verify(
            x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_IsFavouriteMovie_RemoveFromFavourite_ShouldRemoveFromFavourite()
    {
        // Arrange
        string clientId = Guid.NewGuid().ToString();
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, clientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));
        _mockCurrentPrincipalAccessor
            .Setup(x => x.Principal)
            .Returns(claimsPrincipal);

        var userFavourite =
            new UserFavourite
            {
                UserId = clientId,
                MovieId = 456,
            };
        _mockUserFavouriteRepository
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userFavourite);
        _mockUserFavouriteRepository
            .Setup(x => x.DeleteAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovieDetailResult());

        var request = new ToggleFavouriteMovieRequest
        {
            MarkAsFavourite = false,
            MovieId = userFavourite.MovieId
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);

        _mockUserFavouriteRepository.Verify(
            x => x.DeleteAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_IsNotFavouriteMovie_RemoveFromFavourite_ShouldSkipRemoveFromFavourite()
    {
        // Arrange
        string clientId = Guid.NewGuid().ToString();
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, clientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));
        _mockCurrentPrincipalAccessor
            .Setup(x => x.Principal)
            .Returns(claimsPrincipal);

        _mockUserFavouriteRepository
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserFavourite?)null);
        _mockUserFavouriteRepository
            .Setup(x => x.DeleteAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));

        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovieDetailResult());

        var request = new ToggleFavouriteMovieRequest
        {
            MarkAsFavourite = false,
            MovieId = 123
        };

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);

        _mockUserFavouriteRepository.Verify(
            x => x.DeleteAsync(It.IsAny<UserFavourite>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_MissingClientId_ShouldThrowException()
    {
        // Arrange
        string clientId = Guid.NewGuid().ToString();
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity("Bearer"));
        _mockCurrentPrincipalAccessor
            .Setup(x => x.Principal)
            .Returns(claimsPrincipal);

        var request = new ToggleFavouriteMovieRequest
        {
            MarkAsFavourite = true,
            MovieId = 123
        };

        // Act
        var handler = CreateHandler();
        var task = handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => task);
    }
}
