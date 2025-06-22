using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Songs.Queries;

public class GetFilteredSongsQueryHandler : IRequestHandler<GetFilteredSongsQuery, List<Song>>
{
    private readonly IApplicationDbContext _context;

    public GetFilteredSongsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> Handle(GetFilteredSongsQuery request, CancellationToken cancellationToken)
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

        if (!string.IsNullOrEmpty(request.SearchQuery))
        {
            var searchQuery = request.SearchQuery.ToLower();
            query = query.Where(s =>
                s.Title.ToLower().Contains(searchQuery) ||
                s.ArtistName.ToLower().Contains(searchQuery));
        }

        if (request.ArtistId.HasValue)
        {
            query = query.Where(s => s.ArtistId == request.ArtistId);
        }

        // Сортировка
        query = request.SortBy?.ToLower() switch
        {
            "newest" => query.OrderByDescending(s => s.CreatedDate),
            "oldest" => query.OrderBy(s => s.CreatedDate),
            "popular" => query.OrderByDescending(s => s.Likes.Count),
            _ => query.OrderByDescending(s => s.CreatedDate) // По умолчанию сортируем по дате создания (новые первыми)
        };

        return await query.ToListAsync(cancellationToken);
    }
} 