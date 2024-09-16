using Moq;
using MyMovieDb.Users.Specifications;
using System.Linq.Expressions;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;

namespace MyMovieDb.Users;

public class GetFavouriteMoviesHandlerTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    private readonly Mock<IRepository<UserFavourite>> _mockUserFavouriteRepository = new Mock<IRepository<UserFavourite>>();
    private readonly Mock<ICurrentPrincipalAccessor> _mockCurrentPrincipalAccessor = new Mock<ICurrentPrincipalAccessor>();

    private GetFavouriteMoviesHandler CreateHandler()
        => new GetFavouriteMoviesHandler(
            _mockUserFavouriteRepository.Object,
            _mockCurrentPrincipalAccessor.Object)
        {
            LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>()
        };

    [Fact]
    public async Task Handle_HasFavouriteMovies_ShouldReturnResult()
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

        var userFavourites = new List<UserFavourite>
        {
            new UserFavourite
            {
                UserId = clientId,
                MovieId = 123,
            },
            new UserFavourite
            {
                UserId = clientId,
                MovieId = 456,
            },
            new UserFavourite
            {
                UserId = clientId,
                MovieId = 789,
            },
            new UserFavourite
            {
                UserId = Guid.NewGuid().ToString(),
                MovieId = 12345,
            }
        };
        var expectedUserFavourites = userFavourites
            .Where(new FavouriteMoviesForUserSpecification(clientId).ToExpression().Compile())
            .ToList();
        _mockUserFavouriteRepository
            .Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUserFavourites);

        var request = new GetFavouriteMoviesRequest();

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUserFavourites.Count, result.Count());
        Assert.Contains(
            expectedUserFavourites,
            x => result.Any(y => y.UserId == x.UserId && y.MovieId == x.MovieId));
    }

    [Fact]
    public async Task Handle_HasNoFavouriteMovies_ShouldReturnEmptyResult()
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

        var userFavourites = new List<UserFavourite>
        {
            new UserFavourite
            {
                UserId = Guid.NewGuid().ToString(),
                MovieId = 12345,
            }
        };
        var expectedUserFavourites = userFavourites
            .Where(new FavouriteMoviesForUserSpecification(clientId).ToExpression().Compile())
            .ToList();
        _mockUserFavouriteRepository
            .Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserFavourite, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUserFavourites);

        var request = new GetFavouriteMoviesRequest();

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
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

        var request = new GetFavouriteMoviesRequest();

        // Act
        var handler = CreateHandler();
        var task = handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => task);
    }
}
