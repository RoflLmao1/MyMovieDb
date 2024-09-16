namespace MyMovieDb.Adapter.Tmdb;

public class Auth0Options
{
    public const string ConfigurationPath = "Auth:Auth0";

    /// <summary>
    /// Authorization URI for retrieving token
    /// </summary>
    public string TokenUri { get; set; } = null!;
}
