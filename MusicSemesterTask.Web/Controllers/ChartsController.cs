using Microsoft.AspNetCore.Mvc;

namespace MusicSemesterTask.Controllers
{
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}