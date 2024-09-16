using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace MyMovieDb.Movies;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MoviesController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<PagedResultDto<MovieDto>> GetList([FromQuery] int page = 1)
    {
        return _mediator.Send(new GetMoviesRequest
        {
            Page = page
        });
    }

    [HttpGet("{id:int}")]
    public Task<MovieDetailDto> Get([FromRoute] int id)
    {
        return _mediator.Send(new GetMovieRequest(id));
    }
}
