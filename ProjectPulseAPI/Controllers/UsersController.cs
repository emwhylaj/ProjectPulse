using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Shared.Enums;
using System.Security.Claims;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole(UserRole role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term cannot be empty");

            var users = await _userService.SearchUsersAsync(searchTerm);
            return Ok(users);
        }

        [HttpGet("project/{projectId}/members")]
        public async Task<IActionResult> GetProjectMembers(int projectId)
        {
            var users = await _userService.GetProjectMembersAsync(projectId);
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email is unique
            var isUnique = await _userService.IsEmailUniqueAsync(userDto.Email);
            if (!isUnique)
                return BadRequest("Email is already registered");

            var currentUserId = GetCurrentUserId();
            var user = await _userService.CreateUserAsync(userDto, currentUserId);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check authorization - users can only update themselves unless they're admin
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != id && currentUserRole != "Admin")
                return Forbid();

            // Check if email is unique (if being updated)
            if (!string.IsNullOrEmpty(userDto.Email))
            {
                var isUnique = await _userService.IsEmailUniqueAsync(userDto.Email, id);
                if (!isUnique)
                    return BadRequest("Email is already registered");
            }

            var user = await _userService.UpdateUserAsync(id, userDto, currentUserId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserId = GetCurrentUserId();
            var success = await _userService.DeleteUserAsync(id, currentUserId);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var currentUserId = GetCurrentUserId();
            var success = await _userService.DeactivateUserAsync(id, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "User deactivated successfully" });
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var currentUserId = GetCurrentUserId();
            var success = await _userService.ActivateUserAsync(id, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "User activated successfully" });
        }

        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check authorization - users can only change their own password unless they're admin
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != id && currentUserRole != "Admin")
                return Forbid();

            var success = await _userService.ChangePasswordAsync(id, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            
            if (!success)
                return BadRequest("Current password is incorrect");

            return Ok(new { message = "Password changed successfully" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}