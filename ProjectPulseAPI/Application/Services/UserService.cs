using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Infrastructure.DTOs;
using ProjectPulseAPI.Shared.Enums;
using ProjectPulseAPI.Shared.Helpers;

namespace ProjectPulseAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.Select(MapToUserDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<User?> GetUserEntityByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetProjectMembersAsync(int projectId)
        {
            var users = await _unitOfWork.UserRepository.GetProjectMembersAsync(projectId);
            return users.Select(MapToUserDto);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role)
        {
            var users = await _unitOfWork.UserRepository.GetUsersByRoleAsync(role);
            return users.Select(MapToUserDto);
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _unitOfWork.UserRepository.SearchUsersAsync(searchTerm);
            return users.Select(MapToUserDto);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto, int createdBy)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = Helpers.HashPassword(userDto.Password),
                PhoneNumber = userDto.PhoneNumber,
                Role = userDto.Role,
                ProfileImageUrl = userDto.ProfileImageUrl,
                IsActive = true,
                CreatedBy = createdBy.ToString(),
                UpdatedBy = createdBy.ToString()
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToUserDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto, int updatedBy)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(userDto.FirstName))
                user.FirstName = userDto.FirstName;
            if (!string.IsNullOrEmpty(userDto.LastName))
                user.LastName = userDto.LastName;
            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;
            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                user.PhoneNumber = userDto.PhoneNumber;
            if (userDto.Role.HasValue)
                user.Role = userDto.Role.Value;
            if (!string.IsNullOrEmpty(userDto.ProfileImageUrl))
                user.ProfileImageUrl = userDto.ProfileImageUrl;
            if (userDto.IsActive.HasValue)
                user.IsActive = userDto.IsActive.Value;

            user.UpdatedBy = updatedBy.ToString();

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapToUserDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id, int deletedBy)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsDeleted = true;
            user.DeletedBy = deletedBy.ToString();
            user.UpdatedBy = deletedBy.ToString();

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateUserAsync(int id, int deactivatedBy)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedBy = deactivatedBy.ToString();

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateUserAsync(int id, int activatedBy)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = true;
            user.UpdatedBy = activatedBy.ToString();

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            return await _unitOfWork.UserRepository.IsEmailUniqueAsync(email, excludeUserId);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            await _unitOfWork.UserRepository.UpdateLastLoginAsync(userId, DateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = Helpers.HashPassword(newPassword);
            user.UpdatedBy = userId.ToString();

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
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