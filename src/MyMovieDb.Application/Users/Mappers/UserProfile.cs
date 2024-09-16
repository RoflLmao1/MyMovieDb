using AutoMapper;
using MyMovieDb.Adapter.Tmdb.Movies;

namespace MyMovieDb.Users.Mappers;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserFavourite, UserFavouriteMovieDto>();
        CreateMap<MovieDetailResult, UserFavourite>();
    }
}
