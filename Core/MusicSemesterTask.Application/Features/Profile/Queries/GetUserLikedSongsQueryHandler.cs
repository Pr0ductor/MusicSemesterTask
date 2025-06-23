using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Profile.Queries;

public class GetUserLikedSongsQueryHandler : IRequestHandler<GetUserLikedSongsQuery, List<Song>>
{
    private readonly IApplicationDbContext _context;

    public GetUserLikedSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> Handle(GetUserLikedSongsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .Where(s => s.Likes.Any(l => l.User.IdentityId == request.UserId))
            .ToListAsync(cancellationToken);
    }
} 