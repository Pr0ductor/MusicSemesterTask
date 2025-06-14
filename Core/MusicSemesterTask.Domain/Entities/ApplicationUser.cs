using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MusicSemesterTask.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Song> LikedSongs { get; set; } = new List<Song>();
    public ICollection<Artist> FollowedArtists { get; set; } = new List<Artist>();
}