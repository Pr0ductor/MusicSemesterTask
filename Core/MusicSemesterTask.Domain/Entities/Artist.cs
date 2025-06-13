using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MusicSemesterTask.Domain.Entities;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string Country { get; set; }

    // Связь с песнями
    public ICollection<Song> Songs { get; set; } = new List<Song>();

    // Список пользователей, которые подписались на этого артиста
    public ICollection<ApplicationUser> Followers { get; set; } = new List<ApplicationUser>();
}