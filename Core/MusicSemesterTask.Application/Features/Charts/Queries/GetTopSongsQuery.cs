using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Application.Interfaces;

namespace MusicSemesterTask.Application.Features.Charts.Queries;

public class GetTopSongsQuery : IRequest<List<Song>>
{
    public int Limit { get; set; } = 20;
}

public class GetTopSongsQueryHandler : IRequestHandler<GetTopSongsQuery, List<Song>>
{
    private readonly IApplicationDbContext _context;

    public GetTopSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> Handle(GetTopSongsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Songs
            .OrderByDescending(s => s.Likes.Count)
            .Take(request.Limit)
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .ToListAsync(cancellationToken);
    }
} 