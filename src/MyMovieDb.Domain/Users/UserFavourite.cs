using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyMovieDb.Users;

/// <summary>
/// Stores users that have a movie as their favourite
/// </summary>
public partial class UserFavourite : CreationAuditedEntity
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

    public virtual User User { get; set; } = null!;

    public override object?[] GetKeys()
        => new object[] { UserId, MovieId };
}
