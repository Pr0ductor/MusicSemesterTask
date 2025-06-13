using System;
using Microsoft.AspNetCore.Identity;

namespace MusicSemesterTask.Domain.Entities;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    public string Country { get; set; }
    public string CoverUrl { get; set; }
    public string AudioUrl { get; set; }

    // Связь с артистом
    public int ArtistId { get; set; }
    public Artist Artist { get; set; }

    // Кто лайкнул этот трек
    public ICollection<ApplicationUser> LikedByUsers { get; set; } = new List<ApplicationUser>();
}