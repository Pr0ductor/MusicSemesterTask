namespace MusicSemesterTask.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(string name, string email, string password);

    Task<bool> LoginAsync(string email, string password);
}