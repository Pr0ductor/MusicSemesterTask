using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Persistence.Contexts;

namespace MusicSemesterTask.Web.Controllers;

public class ArtistProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    
    
    [HttpGet]
    public async Task<IActionResult> Index(int? id = null)
    {
        return View();
    }
}