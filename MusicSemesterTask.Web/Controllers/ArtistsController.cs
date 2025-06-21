using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Persistence.Repositories;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistsController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public IActionResult Index(string countryFilter)
        {
            var artists = _artistRepository.GetArtists();

            // Применяем фильтр по стране, если он указан
            if (!string.IsNullOrEmpty(countryFilter))
            {
                artists = artists.Where(a => a.Country == countryFilter);
            }

            // Сортируем артистов по количеству подписчиков
            artists = artists.OrderByDescending(a => a.Followers.Count());

            return View(artists.ToList());
        }
    }
}