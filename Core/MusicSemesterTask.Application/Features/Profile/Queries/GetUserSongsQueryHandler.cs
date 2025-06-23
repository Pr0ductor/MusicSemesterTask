using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Entities;
using System.Linq.Expressions;

namespace MusicSemesterTask.Application.Features.Profile.Queries;

public class GetUserSongsQueryHandler : IRequestHandler<GetUserSongsQuery, List<GetUserSongsQueryResult>>
{
    private readonly IApplicationDbContext _context;

    public GetUserSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetUserSongsQueryResult>> Handle(GetUserSongsQuery request, CancellationToken cancellationToken)
    {
        // Получаем треки напрямую по ArtistId
        var query = _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .Where(s => s.ArtistId.ToString() == request.UserId) // Хардкодим ID артиста "Мартин" временно
            .Select(s => new GetUserSongsQueryResult
            {
                Id = s.Id,
                Title = s.Title,
                ArtistName = s.Artist != null ? s.Artist.Name : s.ArtistName,
                CoverUrl = s.CoverUrl,
                AudioUrl = s.AudioUrl,
                Duration = s.Duration,
                IsLiked = s.Likes.Any(l => l.UserId.ToString() == request.UserId),
                Genre = s.Genre,
                Country = s.Country
            });

        return await query.ToListAsync(cancellationToken);
    }
} 