namespace MyMovieDb.Adapter.Tmdb;

public class PagedAndDatedResult<TResult> : PagedResult<TResult>
{
    /// <summary>
    /// Contains the date range of the results.
    /// </summary>
    public DateRange Dates { get; set; } = null!;
}
