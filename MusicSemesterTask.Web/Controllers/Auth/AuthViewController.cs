using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSemesterTask.Application.Features.Auth.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MusicSemesterTask.Web.Controllers.Auth;

public class AuthViewController : Controller
{
    private readonly IMediator _mediator;

    public AuthViewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(command);
        }

        // После успешной регистрации создаем клеймы и устанавливаем аутентификацию
        await SetAuthenticationCookie(result);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(command);
        }

        // Устанавливаем аутентификационные куки
        await SetAuthenticationCookie(result);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SetAuthenticationCookie(AuthCommandResult result)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, result.UserName),
            new Claim(ClaimTypes.Email, result.Email),
            new Claim(ClaimTypes.NameIdentifier, result.UserId),
            new Claim(ClaimTypes.Role, result.Role)
        };

        // Добавляем ProfilePictureUrl только если он не пустой
        if (!string.IsNullOrEmpty(result.ProfilePictureUrl))
        {
            claims.Add(new Claim("ProfilePictureUrl", result.ProfilePictureUrl));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
} 