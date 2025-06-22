
using MusicSemesterTask.Domain.Common;

namespace MusicSemesterTask.Domain.Entities;

public class Like : BaseAuditableEntity
{

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int SongId { get; set; }
    public Song Song { get; set; }
}