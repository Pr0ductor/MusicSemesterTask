using MusicSemesterTask.Domain.Common.Interfaces;

namespace MusicSemesterTask.Domain.Common;


public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}
