using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Web.Controllers
{
    public class ArtistsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}