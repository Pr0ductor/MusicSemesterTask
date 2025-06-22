using System.Security.Claims;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(bool Success, string Message)> RegisterAsync(ApplicationUser user, string password);
    Task<(bool Success, string Token, string Message, string UserName, string UserId, string Role, string? ProfilePictureUrl)> LoginAsync(string email, string password);
    string GenerateJwtToken(IEnumerable<Claim> claims);
}