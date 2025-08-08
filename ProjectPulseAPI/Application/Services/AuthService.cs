using Microsoft.IdentityModel.Tokens;
using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Infrastructure.DTOs;
using ProjectPulseAPI.Shared.Enums;
using ProjectPulseAPI.Shared.Helpers;
using ProjectPulseAPI.Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProjectPulseAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email);
                if (user == null || !user.IsActive)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                await _unitOfWork.UserRepository.UpdateLastLoginAsync(user.Id, DateTime.UtcNow);
                await _unitOfWork.SaveChangesAsync();

                var token = _jwtTokenService.GenerateToken(user);
                var userDto = MapToUserDto(user);

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    User = userDto,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "An error occurred during login",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if email is unique
                var emailExists = !await _unitOfWork.UserRepository.IsEmailUniqueAsync(registerDto.Email);
                if (emailExists)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Email is already registered"
                    };
                }

                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    PasswordHash = Helpers.HashPassword(registerDto.Password),
                    PhoneNumber = registerDto.PhoneNumber,
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var token = _jwtTokenService.GenerateToken(user);
                var userDto = MapToUserDto(user);

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    User = userDto,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            return _jwtTokenService.GenerateToken(user);
        }

        public async Task<User?> GetCurrentUserAsync(string token)
        {
            try
            {
                var principal = _jwtTokenService.ValidateToken(token);
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return null;
                }

                return await _unitOfWork.UserRepository.GetByIdAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return _jwtTokenService.IsTokenValid(token);
        }

        public Task LogoutAsync(string token)
        {
            // In a real implementation, you might want to blacklist the token
            return Task.CompletedTask;
        }

        public async Task<AuthResult> RefreshTokenAsync(string token)
        {
            var user = await GetCurrentUserAsync(token);
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid token"
                };
            }

            var newToken = _jwtTokenService.GenerateToken(user);
            return new AuthResult
            {
                Success = true,
                Token = newToken,
                User = MapToUserDto(user)
            };
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                ProfileImageUrl = user.ProfileImageUrl,
                LastLoginAt = user.LastLoginAt,
                CreatedAt = user.CreatedAt
            };
        }
    }
}