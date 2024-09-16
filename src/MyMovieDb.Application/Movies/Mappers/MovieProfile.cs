using AutoMapper;
using MyMovieDb.Adapter.Tmdb.Movies;

namespace MyMovieDb.Movies.Mappers;

internal class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<MovieResult, MovieDto>();
        CreateMap<MovieBelongsToCollection, MovieBelongsToCollectionDto>();
        CreateMap<MovieDetailResult, MovieDetailDto>();
        CreateMap<MovieGenre, MovieGenreDto>();
        CreateMap<MovieLanguage, MovieLanguageDto>();
        CreateMap<MovieProductionCompany, MovieProductionCompanyDto>();
        CreateMap<MovieProductionCountry, MovieProductionCountryDto>();
    }
}
