using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Songs.Queries;

public class GetFilteredSongsQueryResult
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public Genre Genre { get; set; }
    public Country Country { get; set; }
    public string CoverUrl { get; set; }
    public string AudioUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime? CreatedDate { get; set; }
} 