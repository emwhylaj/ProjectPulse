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
    public class ActivitiesController : ControllerBase
    {
        private readonly IProjectActivityService _activityService;

        public ActivitiesController(IProjectActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectActivities(int projectId, [FromQuery] int limit = 50)
        {
            var activities = await _activityService.GetProjectActivitiesAsync(projectId, limit);
            return Ok(activities);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserActivities(int userId, [FromQuery] int limit = 50)
        {
            // Check authorization - users can only view their own activities unless they're admin/manager
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != userId && !IsAdminOrManager(currentUserRole))
                return Forbid();

            var activities = await _activityService.GetUserActivitiesAsync(userId, limit);
            return Ok(activities);
        }

        [HttpGet("my-activities")]
        public async Task<IActionResult> GetMyActivities([FromQuery] int limit = 50)
        {
            var currentUserId = GetCurrentUserId();
            var activities = await _activityService.GetUserActivitiesAsync(currentUserId, limit);
            return Ok(activities);
        }

        [HttpGet("project/{projectId}/type/{activityType}")]
        public async Task<IActionResult> GetActivitiesByType(int projectId, ActivityType activityType)
        {
            var activities = await _activityService.GetActivitiesByTypeAsync(projectId, activityType);
            return Ok(activities);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int days = 7, [FromQuery] int limit = 100)
        {
            var activities = await _activityService.GetRecentActivitiesAsync(days, limit);
            return Ok(activities);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> LogActivity([FromBody] LogActivityDto activityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _activityService.LogActivityAsync(activityDto);
            return Ok(new { message = "Activity logged successfully" });
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

        private bool IsAdminOrManager(string role)
        {
            return role == "Admin" || role == "ProjectManager";
        }
    }
}