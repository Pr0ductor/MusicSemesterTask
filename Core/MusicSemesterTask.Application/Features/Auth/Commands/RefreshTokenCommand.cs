using MediatR;

namespace MusicSemesterTask.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
} 