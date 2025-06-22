using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Domain.Enums;
using MusicSemesterTask.Persistence.Contexts;
using MusicSemesterTask.Web.Hubs;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MusicSemesterTask.Application.Features.Songs.Commands;
using MusicSemesterTask.Application.Features.Songs.Queries;

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
        
        public async Task<IActionResult> Index(Country? country = null, Genre? genre = null, 
            string? searchQuery = null, string? sortBy = null, int? artistId = null)
        {
            var query = new GetFilteredSongsQuery
            {
                Country = country,
                Genre = genre,
                SearchQuery = searchQuery,
                SortBy = sortBy,
                ArtistId = artistId
            };

            var songs = await _mediator.Send(query);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SongsList", songs);
            }

            return View(songs);
        }
        
        // [HttpPost]
        // public IActionResult Create(Song song)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Songs.Add(song);
        //         _context.SaveChanges();
        //
        //         // Отправка уведомления подписчикам артиста
        //         var artist = _context.Artists.Find(song.ArtistId);
        //         if (artist != null)
        //         {
        //             _hubContext.Clients.Group(artist.Name).SendAsync("ReceiveTrackUpdate", artist.Name, song.Title);
        //         }
        //
        //         return RedirectToAction("Index");
        //     }
        //     return View(song);
        // }
        
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Like(int songId)
        {
            var command = new LikeSongCommand { SongId = songId };
            var result = await _mediator.Send(command);
            return Json(new { success = result });
        }
    }
}