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
        // Сначала найдем пользователя по Id
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == request.UserId, cancellationToken);

        if (user == null)
            return new List<Song>();

        // Теперь получим все лайкнутые песни для этого пользователя
        return await _context.Likes
            .Where(l => l.UserId == user.Id)
            .Include(l => l.Song)
                .ThenInclude(s => s.Artist)
            .Select(l => l.Song)
            .ToListAsync(cancellationToken);
    }
} 