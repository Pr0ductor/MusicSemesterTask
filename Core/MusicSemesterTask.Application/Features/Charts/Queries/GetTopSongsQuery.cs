using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Charts.Queries;

public class GetTopSongsQuery : IRequest<List<Song>>
{
    public int Limit { get; set; } = 20;
    public Country? Country { get; set; }
    public Genre? Genre { get; set; }
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
        var query = _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .AsQueryable();

        if (request.Country.HasValue)
        {
            query = query.Where(s => s.Country == request.Country);
        }

        if (request.Genre.HasValue)
        {
            query = query.Where(s => s.Genre == request.Genre);
        }

        return await query
            .OrderByDescending(s => s.Likes.Count)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
    }
} 