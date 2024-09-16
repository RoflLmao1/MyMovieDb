using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace MyMovieDb.Movies;

public class MovieDetailDto : EntityDto<int>
{
    public bool Adult { get; set; }
    public string BackdropPath { get; set; } = null!;
    public MovieBelongsToCollectionDto? BelongsToCollection { get; set; }
    public int Budget { get; set; }
    public IEnumerable<MovieGenreDto> Genres { get; set; } = Enumerable.Empty<MovieGenreDto>();
    public string Homepage { get; set; } = null!;
    public string ImdbId { get; set; } = null!;
    public string OriginalLanguage { get; set; } = null!;
    public string OriginalTitle { get; set; } = null!;
    public string Overview { get; set; } = null!;
    public double Popularity { get; set; }
    public string PosterPath { get; set; } = null!;
    public IEnumerable<MovieProductionCompanyDto> ProductionCompanies { get; set; } = Enumerable.Empty<MovieProductionCompanyDto>();
    public IEnumerable<MovieProductionCountryDto> ProductionCountries { get; set; } = Enumerable.Empty<MovieProductionCountryDto>();
    public int Revenue { get; set; }
    public string ReleaseDate { get; set; } = null!;
    public int Runtime { get; set; }
    public IEnumerable<MovieLanguageDto> SpokenLanguages { get; set; } = Enumerable.Empty<MovieLanguageDto>();
    public string Status { get; set; } = null!;
    public string Tagline { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool Video { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
}
