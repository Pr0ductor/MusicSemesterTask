using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using MusicSemesterTask.Domain.Common;

namespace MusicSemesterTask.Domain.Entities;

public class ApplicationUser : BaseAuditableEntity
{
    public string IdentityId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

}