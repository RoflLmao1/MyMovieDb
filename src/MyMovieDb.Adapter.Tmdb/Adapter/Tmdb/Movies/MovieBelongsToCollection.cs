namespace MyMovieDb.Adapter.Tmdb.Movies;

public class MovieBelongsToCollection
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PosterPath { get; set; } = null!;
    public string BackdropPath { get; set; } = null!;
}
