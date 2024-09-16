namespace MyMovieDb.Adapter.Tmdb.Movies;

public abstract class MovieBase
{
    public bool Adult { get; set; }
    public string BackdropPath { get; set; } = null!;
    public int Id { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string OriginalTitle { get; set; } = null!;
    public string Overview { get; set; } = null!;
    public double Popularity { get; set; }
    public string PosterPath { get; set; } = null!;
    public string ReleaseDate { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool Video { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
}
