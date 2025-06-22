using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Models;
using MediatR;
using MusicSemesterTask.Application.Features.Home.Queries;

namespace MusicSemesterTask.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            LatestSongs = await _mediator.Send(new GetLatestFiveSongsQuery()),
            TopSongs = await _mediator.Send(new GetTopTwelveSongsQuery())
        };
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}