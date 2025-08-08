using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Infrastructure.DTOs;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginDto loginDto);
        Task<AuthResult> RegisterAsync(RegisterDto registerDto);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<User?> GetCurrentUserAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(string token);
        Task<AuthResult> RefreshTokenAsync(string token);
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public UserDto? User { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}