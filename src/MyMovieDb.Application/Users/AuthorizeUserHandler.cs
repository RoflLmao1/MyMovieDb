using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyMovieDb.Users;

public class AuthorizeUserHandler : ApplicationService,
    IRequestHandler<AuthorizeUserRequest, AuthorizeUserResponse>
{
    private readonly IRepository<User, string> _userRepository;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public AuthorizeUserHandler(
        IRepository<User, string> userRepository,
        IUserAuthorizationService userAuthorizationService)
    {
        _userRepository = userRepository;
        _userAuthorizationService = userAuthorizationService;
    }

    public async Task<AuthorizeUserResponse> Handle(AuthorizeUserRequest request, CancellationToken cancellationToken)
    {
        // Authorize
        var userAuthorizationResult = await _userAuthorizationService.Authorize(new UserAuthorization
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret,
            Audience = request.Audience,
            GrantType = request.GrantType
        });

        // User does not exist
        if (!await _userRepository.AnyAsync(
            x => x.Id == request.ClientId,
            cancellationToken: cancellationToken))
        {
            // Insert a record
            var user = new User(request.ClientId);

            await _userRepository.InsertAsync(user, cancellationToken: cancellationToken);
        }

        return new AuthorizeUserResponse
        {
            AccessToken = userAuthorizationResult.AccessToken,
            TokenType = userAuthorizationResult.TokenType,
            ExpiresIn = userAuthorizationResult.ExpiresIn,
        };
    }
}

public class AuthorizeUserRequest : IRequest<AuthorizeUserResponse>
{
    /// <summary>
    /// Client Id
    /// </summary>
    public string ClientId { get; set; } = null!;
    /// <summary>
    /// Client Secret
    /// </summary>
    public string ClientSecret { get; set; } = null!;
    /// <summary>
    /// Audience
    /// </summary>
    public string Audience { get; set; } = null!;
    /// <summary>
    /// Grant Type
    /// </summary>
    public string GrantType { get; set; } = null!;
}

public class AuthorizeUserResponse
{
    /// <summary>
    ///  Access token
    /// </summary>
    public string AccessToken { get; set; } = null!;
    /// <summary>
    /// Token type
    /// </summary>
    public string TokenType { get; set; } = null!;
    /// <summary>
    /// Expires in (seconds)
    /// </summary>
    public long ExpiresIn { get; set; }
}
