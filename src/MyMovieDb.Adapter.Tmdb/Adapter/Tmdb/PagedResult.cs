using System.Collections.Generic;
using System.Linq;

namespace MyMovieDb.Adapter.Tmdb;

public class PagedResult<TResult>
{
    /// <summary>
    /// Current page.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Collection of results
    /// </summary>
    public IEnumerable<TResult> Results { get; set; }
    public int TotalPages { get; set; }
    public int TotalResults { get; set; }

    public PagedResult()
    {
        Results = Enumerable.Empty<TResult>();
    }
}
