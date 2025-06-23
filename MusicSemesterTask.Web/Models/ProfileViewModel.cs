using MusicSemesterTask.Application.Features.Profile.Queries;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Web.Models;

public class ProfileViewModel
{
    public List<GetUserSongsQueryResult> Songs { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string UserName { get; set; }
    public int SongsCount { get; set; }
    public bool IsCurrentUser { get; set; }
    public bool IsArtist { get; set; }
    public List<Song> LikedSongs { get; set; }
} 