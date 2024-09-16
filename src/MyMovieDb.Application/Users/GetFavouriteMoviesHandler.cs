using MediatR;
using MyMovieDb.Users.Specifications;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;

namespace MyMovieDb.Users;

public class GetFavouriteMoviesHandler : ApplicationService,
    IRequestHandler<GetFavouriteMoviesRequest, IEnumerable<UserFavouriteMovieDto>>
{
    private readonly IRepository<UserFavourite> _userFavouriteRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public GetFavouriteMoviesHandler(
        IRepository<UserFavourite> userFavouriteRepository,
        ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _userFavouriteRepository = userFavouriteRepository;
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }

    public async Task<IEnumerable<UserFavouriteMovieDto>> Handle(GetFavouriteMoviesRequest request, CancellationToken cancellationToken)
    {
        // Get client Id
        string? clientId = _currentPrincipalAccessor.Principal.GetClientId()
            ?? throw new UnauthorizedAccessException();

        // Get user with details
        var userFavourites = await _userFavouriteRepository.GetListAsync(
            new FavouriteMoviesForUserSpecification(clientId),
            cancellationToken: cancellationToken);

        return ObjectMapper.Map<ICollection<UserFavourite>, IEnumerable<UserFavouriteMovieDto>>(
            userFavourites);
    }
}

public class GetFavouriteMoviesRequest : IRequest<IEnumerable<UserFavouriteMovieDto>>
{
}
