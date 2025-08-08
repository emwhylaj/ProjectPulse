using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(int userId);
        Task<Notification> CreateNotificationAsync(CreateNotificationDto notificationDto);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task CleanupOldNotificationsAsync(int daysOld = 30);
        
        // Specific notification creation methods
        Task NotifyTaskAssignedAsync(int userId, int taskId, string taskTitle, int assignedBy);
        Task NotifyTaskStatusChangedAsync(int userId, int taskId, string taskTitle, TaskStatus oldStatus, TaskStatus newStatus);
        Task NotifyProjectMemberAddedAsync(int userId, int projectId, string projectName, int addedBy);
        Task NotifyTaskCommentAddedAsync(int userId, int taskId, string taskTitle, string commenterName);
        Task NotifyTaskDueSoonAsync(int userId, int taskId, string taskTitle, DateTime dueDate);
    }

    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
        public string? RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
    }
}