using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthCommandResult>
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Error { get; set; }
} 