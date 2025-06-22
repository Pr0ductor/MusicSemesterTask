using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Domain.Entities;

namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthCommandResult>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        return new AuthCommandResult
        {
            Success = result.Success,
            Token = result.Token,
            Message = result.Message,
            Email = request.Email,
            UserName = result.UserName,
            UserId = result.UserId,
            Role = result.Role
        };
    }
} 