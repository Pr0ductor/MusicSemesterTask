using MusicSemesterTask.Domain.Entities;
using System.Collections.Generic;

namespace MusicSemesterTask.Persistence.Repositories
{
    public interface IArtistRepository
    {
        IEnumerable<Artist> GetArtists();
    }
}