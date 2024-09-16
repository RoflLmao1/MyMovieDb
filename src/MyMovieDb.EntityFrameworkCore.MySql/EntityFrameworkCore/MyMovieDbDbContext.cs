using Microsoft.EntityFrameworkCore;
using MyMovieDb.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyMovieDb.EntityFrameworkCore;

[ConnectionStringName("MyMovieDb")]
public partial class MyMovieDbDbContext : AbpDbContext<MyMovieDbDbContext>
{
    public MyMovieDbDbContext(DbContextOptions<MyMovieDbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserFavourite> UserFavourites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_0900_ai_ci");

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("Id");

            entity.Property(e => e.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnType("varchar(40)")
                .HasColumnName("ConcurrencyStamp");

            entity.Property(e => e.CreationTime)
                .HasColumnType("datetime(6)")
                .HasColumnName("CreationTime");

            entity.Property(e => e.CreatorId)
                .HasColumnType("char(36)")
                .HasColumnName("CreatorId");

            entity.Property(e => e.ExtraProperties)
                .HasColumnType("longtext")
                .HasColumnName("ExtraProperties");

            entity.Property(e => e.LastModificationTime)
                .HasColumnType("datetime(6)")
                .HasColumnName("LastModificationTime");

            entity.Property(e => e.LastModifierId)
                .HasColumnType("char(36)")
                .HasColumnName("LastModifierId");
        });

        modelBuilder.Entity<UserFavourite>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.MovieId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("UserFavourite");

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("UserId");

            entity.Property(e => e.MovieId).HasColumnName("MovieId");

            entity.Property(e => e.Adult).HasColumnName("Adult");

            entity.Property(e => e.BackdropPath)
                .HasMaxLength(255)
                .HasColumnName("BackdropPath");

            entity.Property(e => e.CreationTime)
                .HasColumnType("datetime(6)")
                .HasColumnName("CreationTime");

            entity.Property(e => e.CreatorId)
                .HasColumnType("char(36)")
                .HasColumnName("CreatorId");

            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(100)
                .HasColumnName("OriginalLanguage");

            entity.Property(e => e.OriginalTitle)
                .HasMaxLength(150)
                .HasColumnName("OriginalTitle");

            entity.Property(e => e.Overview)
                .HasMaxLength(500)
                .HasColumnName("Overview");

            entity.Property(e => e.Popularity).HasColumnName("Popularity");

            entity.Property(e => e.PosterPath)
                .HasMaxLength(255)
                .HasColumnName("PosterPath");

            entity.Property(e => e.ReleaseDate)
                .HasMaxLength(10)
                .HasColumnName("ReleaseDate");

            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("Title");

            entity.Property(e => e.Video).HasColumnName("Video");

            entity.Property(e => e.VoteAverage).HasColumnName("VoteAverage");

            entity.Property(e => e.VoteCount).HasColumnName("VoteCount");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserFavourites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK1_UserFavourite");
        });
    }
}
