using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Application.Interfaces;

namespace MusicSemesterTask.Application.Features.Songs.Commands;

public class LikeSongCommandHandler : IRequestHandler<LikeSongCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public LikeSongCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(LikeSongCommand request, CancellationToken cancellationToken)
    {
        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == int.Parse(request.UserId) && l.SongId == request.SongId, cancellationToken);

        if (existingLike != null)
        {
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new Like
        {
            UserId = int.Parse(request.UserId),
            SongId = request.SongId
        };

        await _context.Likes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 