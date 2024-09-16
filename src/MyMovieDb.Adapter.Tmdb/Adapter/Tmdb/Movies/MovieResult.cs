using System;

namespace MyMovieDb.Adapter.Tmdb.Movies;

public class MovieResult : MovieBase
{
    public int[] GenreIds { get; set; } = Array.Empty<int>();
}
