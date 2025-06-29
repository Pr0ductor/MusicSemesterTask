using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using MediatR;
using MusicSemesterTask.Application.Features.Charts.Queries;
using MusicSemesterTask.Application.Interfaces.Services;
using System.Security.Claims;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;

        public ChartsController(
            ApplicationDbContext context, 
            IAuthService authService, 
            IMediator mediator)
        {
            _context = context;
            _authService = authService;
            _mediator = mediator;
        }

        // GET: /Charts/Index
        public async Task<IActionResult> Index(Country? country = null, Genre? genre = null)
        {
            var query = new GetTopSongsQuery
            {
                Country = country,
                Genre = genre
            };
            
            var songs = await _mediator.Send(query);
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SongsList", songs);
            }
            
            return View(songs);
        }

        // // POST: /Charts/LikeSong
        // [HttpPost]
        // public IActionResult LikeSong(int songId)
        // {
        //     var song = _context.Songs.Find(songId);
        //     if (song == null) return NotFound();
        //
        //     var user = _userManager.GetUserAsync(User).Result;
        //     if (user == null) return Unauthorized();
        //
        //     // Проверяем, поставил ли пользователь лайк
        //     if (!song.LikedByUsers.Contains(user))
        //     {
        //         song.LikedByUsers.Add(user); // Добавляем лайк
        //         _context.SaveChanges();
        //     }
        //
        //     return Ok(new { likesCount = song.LikedByUsers.Count }); // Возвращаем количество лайков
        // }
    }
}