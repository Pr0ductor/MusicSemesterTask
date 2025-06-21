using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using MusicSemesterTask.Web.Hubs;

namespace MusicSemesterTask.Web.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SongsController(
            ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            var songs = _context.Songs.ToList(); // Получаем список песен
            return View(songs); // Передаём их в представление
        }
        
        [HttpPost]
        public IActionResult Create(Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Songs.Add(song);
                _context.SaveChanges();

                // Отправка уведомления подписчикам артиста
                var artist = _context.Artists.Find(song.ArtistId);
                if (artist != null)
                {
                    _hubContext.Clients.Group(artist.Name).SendAsync("ReceiveTrackUpdate", artist.Name, song.Title);
                }

                return RedirectToAction("Index");
            }
            return View(song);
        }
        
        [HttpPost]
        public async Task<IActionResult> LikeSong(int songId)
        {
            var song = await _context.Songs
                .Include(s => s.LikedByUsers)
                .FirstOrDefaultAsync(s => s.Id == songId);

            if (song == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (!song.LikedByUsers.Any(u => u.Id == user.Id))
            {
                song.LikedByUsers.Add(user);
            }
            else
            {
                song.LikedByUsers.Remove(user);
            }

            await _context.SaveChangesAsync();

            return Ok(new { likesCount = song.LikedByUsers.Count });
        }
    }
}