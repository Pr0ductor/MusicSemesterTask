using MusicSemesterTask.Domain.Common;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Domain.Entities;

public class ApplicationUser : BaseAuditableEntity
{
    public string IdentityId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public Country Country { get; set; }
    public UserRole Role { get; set; }
    
    
    // Навигационные свойства
    public virtual ICollection<ArtistSubscription> Subscriptions { get; set; } = new List<ArtistSubscription>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}