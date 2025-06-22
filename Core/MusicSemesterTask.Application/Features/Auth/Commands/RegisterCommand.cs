using MediatR;
using System.ComponentModel.DataAnnotations;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthCommandResult>
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters", MinimumLength = 3)]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be between 2 and 50 characters", MinimumLength = 2)]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name must be between 2 and 50 characters", MinimumLength = 2)]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters", MinimumLength = 6)]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Country is required")]
    public Country Country { get; set; }
    
    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; }
    
    public string? ProfilePictureUrl { get; set; }
}

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }
} 