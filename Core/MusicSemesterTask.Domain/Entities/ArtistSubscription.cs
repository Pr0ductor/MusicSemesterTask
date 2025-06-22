using MusicSemesterTask.Domain.Common;

namespace MusicSemesterTask.Domain.Entities;

public class ArtistSubscription : BaseAuditableEntity
{
    public int UserId { get; set; }
    public int ArtistId { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    public virtual Artist Artist { get; set; }
}