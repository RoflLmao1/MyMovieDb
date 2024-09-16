using System;

namespace MyMovieDb.Adapter.Tmdb.Movies;

public class MovieDetailResult : MovieBase
{
    public MovieBelongsToCollection? BelongsToCollection { get; set; }
    public int Budget { get; set; }
    public MovieGenre[] Genres { get; set; } = Array.Empty<MovieGenre>();
    public string Homepage { get; set; } = null!;
    public string ImdbId { get; set; } = null!;
    public MovieProductionCompany[] ProductionCompanies { get; set; } = Array.Empty<MovieProductionCompany>();
    public MovieProductionCountry[] ProductionCountries { get; set; } = Array.Empty<MovieProductionCountry>();
    public int Revenue { get; set; }
    public int Runtime { get; set; }
    public MovieLanguage[] SpokenLanguages { get; set; } = Array.Empty<MovieLanguage>();
    public string Status { get; set; } = null!;
    public string Tagline { get; set; } = null!;
}
