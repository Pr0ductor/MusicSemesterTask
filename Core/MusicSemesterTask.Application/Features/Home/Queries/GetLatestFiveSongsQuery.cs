using System.Linq;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Common;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Home.Queries;

public record GetLatestFiveSongsQuery : IRequest<IEnumerable<Song>>;

public class GetLatestFiveSongsQueryHandler : IRequestHandler<GetLatestFiveSongsQuery, IEnumerable<Song>>
{
    private readonly IApplicationDbContext _context;

    public GetLatestFiveSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Song>> Handle(GetLatestFiveSongsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Songs
            .OrderByDescending(s => s.CreatedDate)
            .Take(5)
            .Include(s => s.Artist)
            .Include(s => s.Likes)
            .ToListAsync(cancellationToken);
    }
} 