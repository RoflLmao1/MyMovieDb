using Volo.Abp.Application.Dtos;

namespace MyMovieDb.Users;

public class UserFavouriteMovieDto : EntityDto
{
    public string UserId { get; set; } = null!;
    public int MovieId { get; set; }
    public bool Adult { get; set; }
    public string BackdropPath { get; set; } = null!;
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
