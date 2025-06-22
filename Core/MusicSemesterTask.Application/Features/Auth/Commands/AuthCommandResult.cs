namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class AuthCommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string Role { get; set; }
    public string? ProfilePictureUrl { get; set; }
} 