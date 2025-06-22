using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Home.Queries;

public record GetTopTwelveSongsQuery : IRequest<List<Song>>;

public class GetTopTwelveSongsQueryHandler : IRequestHandler<GetTopTwelveSongsQuery, List<Song>>
{
    private readonly IApplicationDbContext _context;

    public GetTopTwelveSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> Handle(GetTopTwelveSongsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .OrderByDescending(s => s.Likes.Count)
            .Take(12)
            .ToListAsync(cancellationToken);
    }
} 