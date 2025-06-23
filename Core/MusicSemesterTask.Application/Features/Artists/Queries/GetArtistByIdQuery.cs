using MediatR;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Artists.Queries;

public class GetArtistByIdQuery : IRequest<ApplicationUser>
{
    public required string Id { get; set; }
} 