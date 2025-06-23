using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Web.Controllers;

public class ArtistProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    
    
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
                    var artist1 = await _context.Artists.FirstOrDefaultAsync(a => a.CreatedBy == user.Id);
                    songs = await _mediator.Send(new GetUserSongsQuery { UserId = artist1.Id.ToString() });
                }

                // Получаем лайкнутые песни
                var likedSongs = await _mediator.Send(new GetUserLikedSongsQuery { UserId = user.Id.ToString() });
                
                return View(new ProfileViewModel
                {
                    Songs = songs,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "../assets/img/question.png",
                    UserName = user.UserName,
                    SongsCount = songs.Count,
                    IsCurrentUser = true,
                    IsArtist = isArtist,
                    LikedSongs = likedSongs
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
                IsArtist = true,
                LikedSongs = new List<Song>() // Empty list for artist profiles
            });
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return RedirectToAction("Error", "Home");
        }
    }
}