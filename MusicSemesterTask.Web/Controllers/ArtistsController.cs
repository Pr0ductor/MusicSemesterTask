using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Domain.Enums;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchQuery = "", Country? country = null, string sortBy = "")
        {
            var query = _context.Artists.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(a => a.Name.Contains(searchQuery));
            }

            if (country.HasValue)
            {
                query = query.Where(a => a.Country == country);
            }

            query = sortBy switch
            {
                "name_desc" => query.OrderByDescending(a => a.Name),
                "name" => query.OrderBy(a => a.Name),
                "subscribers_desc" => query.OrderByDescending(a => a.Subscribers.Count),
                "subscribers" => query.OrderBy(a => a.Subscribers.Count),
                _ => query.OrderBy(a => a.Name)
            };

            var artists = await query.ToListAsync();
            return View(artists);
        }
    }
}