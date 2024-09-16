namespace MyMovieDb.Adapter.Tmdb.Movies;

public class MovieProductionCompany
{
    public int Id { get; set; }
    public string? LogoPath { get; set; }
    public string Name { get; set; } = null!;
    public string OriginCountry { get; set; } = null!;
}
