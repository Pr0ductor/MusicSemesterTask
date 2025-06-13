using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public Microsoft.EntityFrameworkCore.DbSet<Song> Songs { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Artist> Artists { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Many-to-many: User - LikedSongs
        builder.Entity<IdentityUserLogin<string>>().HasKey(u => u.UserId);
        builder.Entity<IdentityUserRole<string>>().HasKey(ur => ur.UserId);
        builder.Entity<IdentityUserToken<string>>().HasKey(t => t.UserId);

        // Many-to-many: ApplicationUser - LikedSongs
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.LikedSongs)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserLikedSongs"));

        // Many-to-many: ApplicationUser - FollowedArtists
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.FollowedArtists)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserFollowedArtists"));
    }
}