using System;
using System.Linq;
using System.Security.Claims;

namespace MyMovieDb.Users;

public static class UserExtensions
{
    public static string CLAIM_TYPE_CLIENT_ID = "azp";

    public static string? GetClientId(this ClaimsPrincipal principal)
    {
        var clientIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == CLAIM_TYPE_CLIENT_ID);
        return clientIdOrNull == null || clientIdOrNull.Value.IsNullOrWhiteSpace() ? null
            : clientIdOrNull.Value;
    }
}
