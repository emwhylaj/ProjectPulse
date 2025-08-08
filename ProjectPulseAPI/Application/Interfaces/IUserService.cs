using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Infrastructure.DTOs;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<User?> GetUserEntityByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetProjectMembersAsync(int projectId);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto, int createdBy);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto, int updatedBy);
        Task<bool> DeleteUserAsync(int id, int deletedBy);
        Task<bool> DeactivateUserAsync(int id, int deactivatedBy);
        Task<bool> ActivateUserAsync(int id, int activatedBy);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task UpdateLastLoginAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }

    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.TeamMember;
        public string? ProfileImageUrl { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole? Role { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}