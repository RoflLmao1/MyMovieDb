using Moq;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Adapter.Tmdb.Movies;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace MyMovieDb.Movies;

public class GetMovieHandlerTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    private readonly Mock<ITmdbService> _mockTmdbService = new Mock<ITmdbService>();

    public GetMovieHandler CreateHandler()
        => new GetMovieHandler(
            _mockTmdbService.Object,
            GetRequiredService<IDistributedCache<MovieDetailDto>>())
        {
            LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>()
        };

    [Fact]
    public async Task Handle_HasMovieDetails_ShouldReturnResult()
    {
        // Arrange
        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovieDetailResult());

        var request = new GetMovieRequest(123);

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        _mockTmdbService.Verify(
            x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidMovie_ShouldThrowCorrectException()
    {
        // Arrange
        _mockTmdbService
            .Setup(x => x.GetMovieDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException(null, null, System.Net.HttpStatusCode.NotFound));

        var request = new GetMovieRequest(123);

        // Act
        var handler = CreateHandler();
        var task = handler.Handle(request, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => task);
        Assert.Equal(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Code, exception.Code);
    }
}
