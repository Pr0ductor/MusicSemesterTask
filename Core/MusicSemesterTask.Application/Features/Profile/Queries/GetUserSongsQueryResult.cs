using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Profile.Queries;

public class GetUserSongsQueryResult
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string CoverUrl { get; set; }
    public string AudioUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsLiked { get; set; }
    public Genre Genre { get; set; }
    public Country Country { get; set; }
} 