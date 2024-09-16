namespace MyMovieDb.Users;

public class UserAuthorizationResult
{
    /// <summary>
    ///  Access token
    /// </summary>
    public string AccessToken { get; set; } = null!;
    /// <summary>
    ///  Token type
    /// </summary>
    public string TokenType { get; set; } = null!;
    /// <summary>
    /// Expires in (seconds)
    /// </summary>
    public long ExpiresIn { get; set; }
}
