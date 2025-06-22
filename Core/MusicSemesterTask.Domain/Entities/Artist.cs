using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MusicSemesterTask.Domain.Common;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Domain.Entities;

public class Artist : BaseAuditableEntity
{
    public string Name { get; set; }
    public string ProfilePictureUrl { get; set; }
    public Country Country { get; set; }
    
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    public virtual ICollection<ArtistSubscription> Subscribers { get; set; } = new List<ArtistSubscription>();
}