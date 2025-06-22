using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Models;

public class HomeViewModel
{
    public IEnumerable<Song> LatestSongs { get; set; }
    public IEnumerable<Song> TopSongs { get; set; }
} 