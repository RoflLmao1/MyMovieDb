namespace MyMovieDb.Adapter.Tmdb;

/// <summary>
/// Class for representing a date range.
/// </summary>
public class DateRange
{
    /// <summary>
    /// Maximum date
    /// </summary>
    public string Maximum { get; set; } = null!;
    /// <summary>
    /// Minimum date
    /// </summary>
    public string Minimum { get; set; } = null!;
}
