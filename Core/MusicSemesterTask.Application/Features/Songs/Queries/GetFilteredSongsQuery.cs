using MediatR;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Songs.Queries;

public class GetFilteredSongsQuery : IRequest<List<Song>>
{
    public Country? Country { get; set; }
    public Genre? Genre { get; set; }
    public string? SearchQuery { get; set; }
    public string? SortBy { get; set; }
    public int? ArtistId { get; set; }
} 