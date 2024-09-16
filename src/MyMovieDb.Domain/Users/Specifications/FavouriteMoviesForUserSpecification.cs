using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace MyMovieDb.Users.Specifications;

public class FavouriteMoviesForUserSpecification : Specification<UserFavourite>
{
    private readonly string _clientId;

    public FavouriteMoviesForUserSpecification(string clientId)
    {
        _clientId = clientId;
    }

    public override Expression<Func<UserFavourite, bool>> ToExpression()
        => x => x.UserId == _clientId;
}
