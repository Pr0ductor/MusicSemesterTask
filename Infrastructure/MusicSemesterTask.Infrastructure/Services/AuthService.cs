using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicSemesterTask.Application.Interfaces.Services;
using MusicSemesterTask.Domain.Entities;
using MusicSemesterTask.Persistence.Contexts;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using MusicSemesterTask.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MusicSemesterTask.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(ApplicationUser user, string password)
    {
        try
        {
            // Проверяем, существует ли пользователь с таким email
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return (false, "User with this email already exists");
            }

            // Проверяем, существует ли пользователь с таким username
            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
            {
                return (false, "User with this username already exists");
            }

            // Хешируем пароль
            user.Password = BC.HashPassword(password);
            
            // Добавляем пользователя в базу данных
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Если пользователь регистрируется как артист, создаем запись в таблице Artists
            if (user.Role == UserRole.Artist)
            {
                var artist = new Artist
                {
                    Name = user.UserName,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "~/assets/img/default-artist.jpg",
                    Country = user.Country,
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.Artists.AddAsync(artist);
                await _context.SaveChangesAsync();
            }
            
            return (true, "User registered successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Registration failed: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Token, string Message, string UserName, string UserId, string Role, string? ProfilePictureUrl)> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return (false, string.Empty, "User not found", string.Empty, string.Empty, string.Empty, null);
            }

            if (!BC.Verify(password, user.Password))
            {
                return (false, string.Empty, "Invalid password", string.Empty, string.Empty, string.Empty, null);
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

            // Добавляем ProfilePictureUrl только если он не пустой
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                // Нормализуем путь к изображению
                var profilePicturePath = user.ProfilePictureUrl;
                if (profilePicturePath.StartsWith("../"))
                {
                    profilePicturePath = profilePicturePath.TrimStart('.');
                }
                claims.Add(new Claim("ProfilePictureUrl", profilePicturePath));
            }

            var token = GenerateJwtToken(claims);

            return (true, token, "Login successful", user.UserName, user.Id.ToString(), user.Role.ToString(), user.ProfilePictureUrl);
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"Login failed: {ex.Message}", string.Empty, string.Empty, string.Empty, null);
        }
    }

    public async Task<string> RefreshTokenAsync(string token)
    {
        // Декодируем текущий токен
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Invalid token"));

        // Находим пользователя
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");

        // Генерируем новый токен
        return GenerateJwtToken(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Email), // Используем email как имя пользователя
            new(ClaimTypes.Role, user.Role.ToString())
        });
    }

    public string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not found in configuration")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;

        return await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }
}