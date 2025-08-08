using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface IProjectActivityService
    {
        Task<IEnumerable<ProjectActivity>> GetProjectActivitiesAsync(int projectId, int limit = 50);
        Task<IEnumerable<ProjectActivity>> GetUserActivitiesAsync(int userId, int limit = 50);
        Task<IEnumerable<ProjectActivity>> GetActivitiesByTypeAsync(int projectId, ActivityType activityType);
        Task<IEnumerable<ProjectActivity>> GetRecentActivitiesAsync(int days = 7, int limit = 100);
        Task LogActivityAsync(LogActivityDto activityDto);
        Task CleanupOldActivitiesAsync(int daysOld = 90);
        
        // Specific activity logging methods
        Task LogProjectCreatedAsync(int projectId, int userId, string projectName);
        Task LogProjectUpdatedAsync(int projectId, int userId, string projectName, string changes);
        Task LogTaskCreatedAsync(int projectId, int userId, string taskTitle);
        Task LogTaskUpdatedAsync(int projectId, int userId, string taskTitle, string changes);
        Task LogTaskStatusChangedAsync(int projectId, int userId, string taskTitle, TaskStatus oldStatus, TaskStatus newStatus);
        Task LogProjectMemberAddedAsync(int projectId, int userId, string memberName, ProjectMemberRole role);
        Task LogProjectMemberRemovedAsync(int projectId, int userId, string memberName);
        Task LogTaskAssignedAsync(int projectId, int userId, string taskTitle, string assigneeName);
        Task LogTaskCommentAddedAsync(int projectId, int userId, string taskTitle);
    }

    public class LogActivityDto
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}