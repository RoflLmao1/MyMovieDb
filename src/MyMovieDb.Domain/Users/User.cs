using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyMovieDb.Users;

public class User : AuditedAggregateRoot<string>
{
    protected User()
    {
        UserFavourites = new HashSet<UserFavourite>();
    }

    public User(string id) : base(id)
    {
        UserFavourites = new HashSet<UserFavourite>();
    }

    public virtual ICollection<UserFavourite> UserFavourites { get; set; }
}
