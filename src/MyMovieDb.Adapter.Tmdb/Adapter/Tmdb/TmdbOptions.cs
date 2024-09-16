namespace MyMovieDb.Adapter.Tmdb;

public class TmdbOptions
{
    public const string ConfigurationPath = "Adapter:Tmdb";

    /// <summary>
    /// Base URI
    /// </summary>
    public string BaseUri { get; set; } = null!;
    /// <summary>
    /// Http Timeout in seconds. Default: 100
    /// </summary>
    public int Timeout { get; set; } = 100;
    /// <summary>
    /// Access token to call TMDB APIs
    /// </summary>
    public string AccessToken { get; set; } = null!;
}
