using System.Security.Claims;
using MediatR;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Domain.Enums;

namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthCommandResult>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Country = request.Country,
            Role = request.Role,
            ProfilePictureUrl = request.ProfilePictureUrl,
            IdentityId = Guid.NewGuid().ToString()
        };

        var result = await _authService.RegisterAsync(user, request.Password);

        if (!result.Success)
        {
            return new AuthCommandResult
            {
                Success = false,
                Message = result.Message
            };
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var token = _authService.GenerateJwtToken(claims);

        return new AuthCommandResult
        {
            Success = true,
            Token = token,
            Message = "Registration successful",
            UserName = user.UserName,
            Email = user.Email,
            UserId = user.Id.ToString(),
            Role = user.Role.ToString()
        };
    }
} 