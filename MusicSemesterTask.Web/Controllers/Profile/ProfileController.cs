using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Application.Features.Profile.Queries;
using MusicSemesterTask.Web.Models;

namespace MusicSemesterTask.Web.Controllers.Profile;

[Authorize]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "AuthView");
        }

        var userSongsQuery = new GetUserSongsQuery { UserId = userId };
        var userSongs = await _mediator.Send(userSongsQuery);

        var likedSongsQuery = new GetUserLikedSongsQuery { UserId = userId };
        var likedSongs = await _mediator.Send(likedSongsQuery);

        var viewModel = new ProfileViewModel
        {
            UserName = User.Identity?.Name ?? "Unknown",
            ProfilePictureUrl = User.Claims.FirstOrDefault(c => c.Type == "ProfilePictureUrl")?.Value,
            IsCurrentUser = true,
            IsArtist = userSongs.Any(),
            Songs = userSongs,
            SongsCount = userSongs.Count,
            LikedSongs = likedSongs
        };

        return View(viewModel);
    }
} 