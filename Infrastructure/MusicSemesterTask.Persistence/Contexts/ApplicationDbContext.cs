using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Application.Interfaces;

namespace MusicSemesterTask.Persistence.Contexts;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Like> Likes { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ArtistSubscription> ArtistSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация для Like
        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Song)
            .WithMany(s => s.Likes)
            .HasForeignKey(l => l.SongId);

        // Уникальный индекс для предотвращения дублирования лайков
        modelBuilder.Entity<Like>()
            .HasIndex(l => new { l.UserId, l.SongId })
            .IsUnique();

        // Конфигурация для ArtistSubscription
        modelBuilder.Entity<ArtistSubscription>()
            .HasKey(a => new { a.UserId, a.ArtistId });

        modelBuilder.Entity<ArtistSubscription>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<ArtistSubscription>()
            .HasOne<Artist>()
            .WithMany(a => a.Subscribers)
            .HasForeignKey(a => a.ArtistId);
    }
}