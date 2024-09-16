using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace MyMovieDb.Users;

public interface IUserAuthorizationService : IDomainService
{
    /// <summary>
    /// Performs authorization using the specified input.
    /// </summary>
    /// <param name="userAuthorization">User authorization data</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> object.</param>
    /// <returns>A <see cref="UserAuthorizationResult"/> instance containing the result of the action.</returns>
    Task<UserAuthorizationResult> Authorize(UserAuthorization userAuthorization, CancellationToken cancellationToken = default);
}
