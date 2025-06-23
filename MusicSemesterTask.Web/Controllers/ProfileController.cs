using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Application.Features.Artists.Queries;
using MusicSemesterTask.Application.Features.Songs.Queries;
using MusicSemesterTask.Application.Features.Profile.Queries;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Domain.Enums;
using MusicSemesterTask.Web.Models;

namespace MusicSemesterTask.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public ProfileController(IAuthService authService, IMediator mediator)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? id = null)
    {
        try
        {
            if (!id.HasValue)
            {
                // Показываем профиль текущего пользователя
                var user = await _authService.GetCurrentUserAsync();
                if (user == null)
                    return RedirectToAction("Login", "AuthView");

                var songs = new List<GetUserSongsQueryResult>();
                var isArtist = user.Role == UserRole.Artist;
                
                if (isArtist)
                {
                    songs = await _mediator.Send(new GetUserSongsQuery { UserId = id.ToString() });
                }
                
                return View(new ProfileViewModel
                {
                    Songs = songs,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "../assets/img/question.png",
                    UserName = user.UserName,
                    SongsCount = songs.Count,
                    IsCurrentUser = true,
                    IsArtist = isArtist
                });
            }
            
            // Показываем профиль артиста
            var artist = await _mediator.Send(new GetArtistByIdQuery { Id = id.Value.ToString() });
            if (artist == null)
                return NotFound();

            var artistSongs = await _mediator.Send(new GetUserSongsQuery { UserId = id.Value.ToString() });
            var currentUser = await _authService.GetCurrentUserAsync();
            
            return View(new ProfileViewModel
            {
                Songs = artistSongs,
                ProfilePictureUrl = artist.ProfilePictureUrl ?? "../assets/img/question.png",
                UserName = artist.UserName,
                SongsCount = artistSongs.Count,
                IsCurrentUser = currentUser?.Id == id.Value,
                IsArtist = true
            });
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return RedirectToAction("Error", "Home");
        }
    }
}