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
            var songs = _context.Songs
                .Include(s => s.Likes) // Загружаем лайки
                .ToList();
            return View(songs);
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
        public async Task<IActionResult> ToggleLike(int songId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.SongId == songId);

            if (existingLike != null)
            {
                // Удаляем лайк, если он уже существует
                _context.Likes.Remove(existingLike);
            }
            else
            {
                // Добавляем новый лайк
                var like = new Like
                {
                    UserId = user.Id,
                    SongId = songId
                };
                _context.Likes.Add(like);
            }

            await _context.SaveChangesAsync();

            var likesCount = await _context.Likes
                .Where(l => l.SongId == songId)
                .CountAsync();

            return Ok(new { liked = existingLike == null, likesCount });
        }
    }
}