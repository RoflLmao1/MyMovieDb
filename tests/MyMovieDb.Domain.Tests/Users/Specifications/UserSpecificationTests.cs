namespace MyMovieDb.Users.Specifications;

public class UserSpecificationTests : MyMovieDbDomainTestBase<MyMovieDbDomainTestModule>
{
    [Fact]
    public void FavouriteMoviesForUserSpecification_ShouldReturnTrue()
    {
        // Arrange
        var userfavourite = new UserFavourite
        {
            UserId = Guid.NewGuid().ToString()
        };

        // Act
        var specification = new FavouriteMoviesForUserSpecification(userfavourite.UserId);

        // Assert
        Assert.True(specification.IsSatisfiedBy(userfavourite));
    }

    [Fact]
    public void FavouriteMoviesForUserSpecification_ShouldReturnFalse()
    {
        // Arrange
        var userfavourite = new UserFavourite
        {
            UserId = Guid.NewGuid().ToString()
        };

        // Act
        var specification = new FavouriteMoviesForUserSpecification(Guid.NewGuid().ToString());

        // Assert
        Assert.False(specification.IsSatisfiedBy(userfavourite));
    }
}
