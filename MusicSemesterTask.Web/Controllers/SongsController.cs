using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Web.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var songs = _context.Songs.ToList();
            return View(songs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Songs.Add(song);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }
    }
}