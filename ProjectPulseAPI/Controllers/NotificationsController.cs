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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications([FromQuery] bool unreadOnly = false)
        {
            var currentUserId = GetCurrentUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(currentUserId, unreadOnly);
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var currentUserId = GetCurrentUserId();
            var count = await _notificationService.GetUnreadCountAsync(currentUserId);
            return Ok(new { count });
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetNotificationsByType(NotificationType type)
        {
            var currentUserId = GetCurrentUserId();
            var notifications = await _notificationService.GetNotificationsByTypeAsync(currentUserId, type);
            return Ok(notifications);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto notificationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notification = await _notificationService.CreateNotificationAsync(notificationDto);
            return CreatedAtAction(nameof(GetUserNotifications), new { id = notification.Id }, notification);
        }

        [HttpPatch("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            // Users can only mark their own notifications as read
            var currentUserId = GetCurrentUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(currentUserId);
            
            if (!notifications.Any(n => n.Id == id))
                return NotFound();

            var success = await _notificationService.MarkAsReadAsync(id);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Notification marked as read" });
        }

        [HttpPatch("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var currentUserId = GetCurrentUserId();
            var success = await _notificationService.MarkAllAsReadAsync(currentUserId);
            
            if (!success)
                return BadRequest("Failed to mark notifications as read");

            return Ok(new { message = "All notifications marked as read" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            // Users can only delete their own notifications
            var currentUserId = GetCurrentUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(currentUserId);
            
            if (!notifications.Any(n => n.Id == id))
                return NotFound();

            var success = await _notificationService.DeleteNotificationAsync(id);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }
    }
}