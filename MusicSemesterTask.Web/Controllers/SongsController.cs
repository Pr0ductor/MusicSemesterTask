using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using MusicSemesterTask.Web.Hubs;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MusicSemesterTask.Application.Features.Songs.Commands;

namespace MusicSemesterTask.Web.Controllers
{
    [Authorize]
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMediator _mediator;

        public SongsController(
            ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext,
            IMediator mediator)
        {
            _context = context;
            _hubContext = hubContext;
            _mediator = mediator;
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
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            
            var command = new LikeSongCommand
            {
                SongId = songId,
                UserId = userId
            };

            var isLiked = await _mediator.Send(command);
            return Json(new { success = true, isLiked });
        }
    }
}