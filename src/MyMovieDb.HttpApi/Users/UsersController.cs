using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MyMovieDb.Users;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("authorize")]
    public Task<AuthorizeUserResponse> Authorize([FromBody] AuthorizeUserRequest request)
    {
        return _mediator.Send(request);
    }

    [HttpGet("favourite-movies")]
    public Task<IEnumerable<UserFavouriteMovieDto>> GetFavouriteMovies()
    {
        return _mediator.Send(new GetFavouriteMoviesRequest());
    }

    [HttpPost("favourite-movies")]
    public Task<bool> ToggleFavouriteMovie([FromBody] ToggleFavouriteMovieRequest request)
    {
        return _mediator.Send(request);
    }
}
