using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<ArtistSubscription> ArtistSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Уникальный индекс для предотвращения дублирования лайков
        builder.Entity<Like>()
            .HasIndex(l => new { l.UserId, l.SongId })
            .IsUnique();

        // Составной первичный ключ для ArtistSubscription
        builder.Entity<ArtistSubscription>()
            .HasKey(a => new { a.UserId, a.ArtistId });

        // Настройка связей для ArtistSubscription
        builder.Entity<ArtistSubscription>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(a => a.UserId);

        builder.Entity<ArtistSubscription>()
            .HasOne<Artist>()
            .WithMany(a => a.Subscribers)
            .HasForeignKey(a => a.ArtistId);
    }
}