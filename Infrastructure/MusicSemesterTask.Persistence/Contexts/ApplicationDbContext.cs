using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Many-to-many: ApplicationUser - LikedSongs
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.LikedSongs)
            .WithMany(s => s.LikedByUsers)
            .UsingEntity(j => j.ToTable("UserLikedSongs"));

        // Many-to-many: ApplicationUser - FollowedArtists
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.FollowedArtists)
            .WithMany(a => a.Followers)
            .UsingEntity(j => j.ToTable("UserFollows"));
    }
}