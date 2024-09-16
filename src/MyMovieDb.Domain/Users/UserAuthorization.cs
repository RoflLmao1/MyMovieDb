namespace MyMovieDb.Users;

public class UserAuthorization
{
    /// <summary>
    /// Client id
    /// </summary>
    public string ClientId { get; set; } = null!;
    /// <summary>
    /// Client secret
    /// </summary>
    public string ClientSecret { get; set; } = null!;
    /// <summary>
    /// Audience
    /// </summary>
    public string Audience { get; set; } = null!;
    /// <summary>
    /// Grant type
    /// </summary>
    public string GrantType { get; set; } = null!;
}
