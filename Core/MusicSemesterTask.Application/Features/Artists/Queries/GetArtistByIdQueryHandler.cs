using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Application.Interfaces;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Artists.Queries;

public class GetArtistByIdQueryHandler : IRequestHandler<GetArtistByIdQuery, ApplicationUser>
{
    private readonly IApplicationDbContext _context;

    public GetArtistByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
    {
        var id = int.Parse(request.Id);
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
} 