namespace MyMovieDb.Movies;

public class MovieBelongsToCollectionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PosterPath { get; set; } = null!;
    public string BackdropPath { get; set; } = null!;
}
