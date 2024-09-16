namespace MyMovieDb.Movies;

public class MovieProductionCompanyDto
{
    public int Id { get; set; }
    public string? LogoPath { get; set; }
    public string Name { get; set; } = null!;
    public string OriginCountry { get; set; } = null!;
}
