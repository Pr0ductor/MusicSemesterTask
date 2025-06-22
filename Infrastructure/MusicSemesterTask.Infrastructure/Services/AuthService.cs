using Microsoft.AspNetCore.Identity;
using MusicSemesterTask.Persistence.Contexts;

namespace Market.Infrastructure.Services;

public class AuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<IdentityUser> userManager, ApplicationDbContext context,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
}