using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace MusicSemesterTask.Persistence.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Artist> GetArtists()
        {
            return _context.Artists.ToList();
        }
    }
}