using Moq;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Adapter.Tmdb.Movies;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace MyMovieDb.Movies;

public class GetMoviesHandlerTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    private readonly Mock<ITmdbService> _mockTmdbService = new Mock<ITmdbService>();

    public GetMoviesHandler CreateHandler()
        => new GetMoviesHandler(
            _mockTmdbService.Object,
            GetRequiredService<IDistributedCache<PagedResultDto<MovieDto>>>())
        {
            LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>()
        };

    [Fact]
    public async Task Handle_HasMovies_ShouldReturnResult()
    {
        // Arrange
        var expectedMovieResults = new List<MovieResult>
        {
            new MovieResult
            {
                Id = 123
            }
        };
        _mockTmdbService
            .Setup(x => x.GetNowPlayingMovies(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedAndDatedResult<MovieResult>
            {
                Dates = new DateRange(),
                Page = 1,
                Results = expectedMovieResults,
                TotalPages = 1,
                TotalResults = 1
            });

        var request = new GetMoviesRequest();

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMovieResults.Count, result.TotalCount);
        Assert.NotNull(result.Items);
        Assert.Equal(expectedMovieResults.Count, result.Items.Count);
        Assert.Contains(
            expectedMovieResults,
            x => result.Items.Any(y => y.Id == x.Id));
    }   

    [Fact]
    public async Task Handle_HasNoMovies_ShouldReturnEmptyResult()
    {
        // Arrange
        _mockTmdbService
            .Setup(x => x.GetNowPlayingMovies(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedAndDatedResult<MovieResult>
            {
                Dates = new DateRange(),
                Page = 1,
                Results = new List<MovieResult>(),
                TotalPages = 1,
                TotalResults = 0
            });

        var request = new GetMoviesRequest();

        // Act
        var handler = CreateHandler();
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalCount);
        Assert.NotNull(result.Items);
        Assert.Empty(result.Items);
    }   
}
