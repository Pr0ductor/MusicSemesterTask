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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityId == request.UserId, cancellationToken);

        if (user == null)
        {
            return false;
        }

        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == user.Id && l.SongId == request.SongId, cancellationToken);

        if (existingLike != null)
        {
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new Like
        {
            UserId = user.Id,
            SongId = request.SongId
        };

        await _context.Likes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 