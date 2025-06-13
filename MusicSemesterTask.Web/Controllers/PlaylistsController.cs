using Microsoft.AspNetCore.Mvc;

namespace MusicSemesterTask.Controllers
{
    public class PlaylistsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}