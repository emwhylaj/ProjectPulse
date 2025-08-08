using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            return await _unitOfWork.NotificationRepository.GetUserNotificationsAsync(userId, unreadOnly);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _unitOfWork.NotificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task<Notification> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Type = notificationDto.Type,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                ActionUrl = notificationDto.ActionUrl,
                RelatedEntityType = notificationDto.RelatedEntityType,
                RelatedEntityId = notificationDto.RelatedEntityId,
                IsRead = false,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return notification;
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            await _unitOfWork.NotificationRepository.MarkAsReadAsync(notificationId);
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            await _unitOfWork.NotificationRepository.MarkAllAsReadAsync(userId);
            return true;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type)
        {
            return await _unitOfWork.NotificationRepository.GetNotificationsByTypeAsync(userId, type);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
            if (notification == null) return false;

            notification.IsDeleted = true;
            await _unitOfWork.NotificationRepository.UpdateAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task CleanupOldNotificationsAsync(int daysOld = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            await _unitOfWork.NotificationRepository.DeleteOldNotificationsAsync(cutoffDate);
        }

        public async Task NotifyTaskAssignedAsync(int userId, int taskId, string taskTitle, int assignedBy)
        {
            var assignerUser = await _unitOfWork.UserRepository.GetByIdAsync(assignedBy);
            var assignerName = assignerUser != null ? $"{assignerUser.FirstName} {assignerUser.LastName}" : "Someone";

            await CreateNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Type = NotificationType.TaskAssigned,
                Title = "New Task Assigned",
                Message = $"{assignerName} has assigned you a new task: {taskTitle}",
                ActionUrl = $"/tasks/{taskId}",
                RelatedEntityType = "Task",
                RelatedEntityId = taskId
            });
        }

        public async Task NotifyTaskStatusChangedAsync(int userId, int taskId, string taskTitle, TaskStatus oldStatus, TaskStatus newStatus)
        {
            await CreateNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Type = NotificationType.TaskStatusChanged,
                Title = "Task Status Updated",
                Message = $"Task '{taskTitle}' status changed from {oldStatus} to {newStatus}",
                ActionUrl = $"/tasks/{taskId}",
                RelatedEntityType = "Task",
                RelatedEntityId = taskId
            });
        }

        public async Task NotifyProjectMemberAddedAsync(int userId, int projectId, string projectName, int addedBy)
        {
            var adderUser = await _unitOfWork.UserRepository.GetByIdAsync(addedBy);
            var adderName = adderUser != null ? $"{adderUser.FirstName} {adderUser.LastName}" : "Someone";

            await CreateNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Type = NotificationType.ProjectMemberAdded,
                Title = "Added to Project",
                Message = $"{adderName} has added you to the project: {projectName}",
                ActionUrl = $"/projects/{projectId}",
                RelatedEntityType = "Project",
                RelatedEntityId = projectId
            });
        }

        public async Task NotifyTaskCommentAddedAsync(int userId, int taskId, string taskTitle, string commenterName)
        {
            await CreateNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Type = NotificationType.TaskCommentAdded,
                Title = "New Comment on Task",
                Message = $"{commenterName} commented on task: {taskTitle}",
                ActionUrl = $"/tasks/{taskId}",
                RelatedEntityType = "Task",
                RelatedEntityId = taskId
            });
        }

        public async Task NotifyTaskDueSoonAsync(int userId, int taskId, string taskTitle, DateTime dueDate)
        {
            var daysUntilDue = (dueDate - DateTime.UtcNow).Days;
            var dueDateText = daysUntilDue == 0 ? "today" : 
                             daysUntilDue == 1 ? "tomorrow" : 
                             $"in {daysUntilDue} days";

            await CreateNotificationAsync(new CreateNotificationDto
            {
                UserId = userId,
                Type = NotificationType.TaskDueSoon,
                Title = "Task Due Soon",
                Message = $"Task '{taskTitle}' is due {dueDateText}",
                ActionUrl = $"/tasks/{taskId}",
                RelatedEntityType = "Task",
                RelatedEntityId = taskId
            });
        }
    }
}