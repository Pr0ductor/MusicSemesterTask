using MediatR;

namespace MusicSemesterTask.Application.Features.Songs.Commands;

public class LikeSongCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public int SongId { get; set; }
}

public class LikeSongCommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public bool IsLiked { get; set; }
} 