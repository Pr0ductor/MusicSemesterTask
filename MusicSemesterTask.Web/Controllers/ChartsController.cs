using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Charts/Index
        public IActionResult Index()
        {
            // Получаем топ-20 песен по количеству лайков
            // var topSongs = _context.Songs
            //     .OrderByDescending(s => s.LikedByUsers.Count) // Сортировка по количеству лайков
            //     .Take(20) // Берём первые 20 песен
            //     .ToList();

            // return View(topSongs
            return View();
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