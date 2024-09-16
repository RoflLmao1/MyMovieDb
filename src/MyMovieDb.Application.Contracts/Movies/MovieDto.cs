using System;
using Volo.Abp.Application.Dtos;

namespace MyMovieDb.Movies;

public class MovieDto : EntityDto<int>
{
    public bool Adult { get; set; }
    public string BackdropPath { get; set; } = null!;
    public int[] GenreIds { get; set; } = Array.Empty<int>();
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
