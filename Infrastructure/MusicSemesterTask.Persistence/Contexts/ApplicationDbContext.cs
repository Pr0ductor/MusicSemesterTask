using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<ArtistSubscription> ArtistSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Уникальный индекс для предотвращения дублирования лайков
        builder.Entity<Like>()
            .HasIndex(l => new { l.UserId, l.SongId })
            .IsUnique();
    }
}