using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MusicSemesterTask.Domain.Common;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Domain.Entities;

public class Artist :BaseAuditableEntity
{
    public string Name { get; set; }
    public string ProfilePictureUrl { get; set; }
    public Country Country { get; set; }
    
}