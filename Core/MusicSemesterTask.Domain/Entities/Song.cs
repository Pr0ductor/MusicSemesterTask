using System;
using Microsoft.AspNetCore.Identity;
using MusicSemesterTask.Domain.Common;
using MusicSemesterTask.Domain.Common.Interfaces;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Domain.Entities;

public class Song : BaseAuditableEntity
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public Genre Genre { get; set; }
    public Country Country { get; set; }
    public string CoverUrl { get; set; }
    public string AudioUrl { get; set; }
    public TimeSpan Duration { get; set; }

    // Связь с артистом
    public int? ArtistId { get; set; }
    public Artist Artist { get; set; }
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}