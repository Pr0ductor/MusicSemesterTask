using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using MusicSemesterTask.Web.Hubs;

namespace MusicSemesterTask.Web.Controllers
{
    [Route("[controller]")]
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ArtistsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        
        public IActionResult Index(string countryFilter)
        {
            var artists = _context.Artists.AsQueryable();

            if (!string.IsNullOrEmpty(countryFilter))
            {
                artists = artists.Where(a => a.Country == countryFilter);
            }

            return View(artists.ToList());
        }

        [HttpPost("follow")]
        public async Task<IActionResult> FollowArtist(int artistId)
        {
            var artist = await _context.Artists.FindAsync(artistId);
            if (artist == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Проверяем, подписан ли пользователь уже
            if (!user.FollowedArtists.Contains(artist))
            {
                user.FollowedArtists.Add(artist);
                artist.Followers.Add(user);

                await _context.SaveChangesAsync();
            }

            // Отправляем уведомление через SignalR
            await _hubContext.Clients.Group(artist.Name).SendAsync("ReceiveTrackUpdate", artist.Name, $"Новый фан подписался: {user.UserName}");

            return Ok(new { Message = $"Вы подписались на {artist.Name}" });
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> UnFollowArtist(int artistId)
        {
            var artist = await _context.Artists.FindAsync(artistId);
            if (artist == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (user.FollowedArtists.Contains(artist))
            {
                user.FollowedArtists.Remove(artist);
                artist.Followers.Remove(user);

                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = $"Вы отписались от {artist.Name}" });
        }
    }
}