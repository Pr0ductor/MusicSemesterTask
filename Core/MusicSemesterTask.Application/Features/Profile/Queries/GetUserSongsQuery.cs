using MediatR;

namespace MusicSemesterTask.Application.Features.Profile.Queries;

public class GetUserSongsQuery : IRequest<List<GetUserSongsQueryResult>>
{
    public string UserId { get; set; }
} 