using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using MusicSemesterTask.Web.Hubs;
using System.Security.Claims;

namespace MusicSemesterTask.Web.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public SongsController(
            ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        
        public async Task<IActionResult> Index(string searchQuery = "", int? artistId = null, string sortBy = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var query = _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Likes)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(s => s.Title.Contains(searchQuery) || s.Artist.Name.Contains(searchQuery));
            }

            if (artistId.HasValue)
            {
                query = query.Where(s => s.ArtistId == artistId.Value);
            }

            query = sortBy.ToLower() switch
            {
                "title" => query.OrderBy(s => s.Title),
                "artist" => query.OrderBy(s => s.Artist.Name),
                "likes" => query.OrderByDescending(s => s.Likes.Count()),
                _ => query.OrderByDescending(s => s.Id)
            };

            var songs = await query.ToListAsync();
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
        public async Task<IActionResult> Like(int songId)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null)
                return NotFound();

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.SongId == songId);

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
            }
            else
            {
                var like = new Like
                {
                    UserId = user.Id,
                    SongId = songId
                };
                _context.Likes.Add(like);
            }

            await _context.SaveChangesAsync();

            var likesCount = await _context.Likes.Where(l => l.SongId == songId).CountAsync();
            return Json(new { likesCount });
        }
    }
}