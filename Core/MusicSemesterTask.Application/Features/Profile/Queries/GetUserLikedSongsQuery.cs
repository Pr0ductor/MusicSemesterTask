using MediatR;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Profile.Queries;

public class GetUserLikedSongsQuery : IRequest<List<Song>>
{
    public string UserId { get; set; }
} 