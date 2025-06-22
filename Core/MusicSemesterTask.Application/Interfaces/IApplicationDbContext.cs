using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Like> Likes { get; set; }
    DbSet<Song> Songs { get; set; }
    DbSet<Artist> Artists { get; set; }
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<ArtistSubscription> ArtistSubscriptions { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 