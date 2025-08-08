using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Shared.Enums;
using System.Text.Json;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Services
{
    public class ProjectActivityService : IProjectActivityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProjectActivity>> GetProjectActivitiesAsync(int projectId, int limit = 50)
        {
            return await _unitOfWork.ProjectActivityRepository.GetProjectActivitiesAsync(projectId, limit);
        }

        public async Task<IEnumerable<ProjectActivity>> GetUserActivitiesAsync(int userId, int limit = 50)
        {
            return await _unitOfWork.ProjectActivityRepository.GetUserActivitiesAsync(userId, limit);
        }

        public async Task<IEnumerable<ProjectActivity>> GetActivitiesByTypeAsync(int projectId, ActivityType activityType)
        {
            return await _unitOfWork.ProjectActivityRepository.GetActivitiesByTypeAsync(projectId, activityType);
        }

        public async Task<IEnumerable<ProjectActivity>> GetRecentActivitiesAsync(int days = 7, int limit = 100)
        {
            return await _unitOfWork.ProjectActivityRepository.GetRecentActivitiesAsync(days, limit);
        }

        public async Task LogActivityAsync(LogActivityDto activityDto)
        {
            await _unitOfWork.ProjectActivityRepository.LogActivityAsync(
                activityDto.ProjectId,
                activityDto.UserId,
                activityDto.ActivityType,
                activityDto.Description,
                activityDto.EntityType,
                activityDto.EntityId,
                activityDto.OldValues,
                activityDto.NewValues
            );
        }

        public async Task CleanupOldActivitiesAsync(int daysOld = 90)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            await _unitOfWork.ProjectActivityRepository.DeleteOldActivitiesAsync(cutoffDate);
        }

        public async Task LogProjectCreatedAsync(int projectId, int userId, string projectName)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.ProjectCreated,
                Description = $"Project '{projectName}' was created",
                EntityType = "Project",
                EntityId = projectId
            });
        }

        public async Task LogProjectUpdatedAsync(int projectId, int userId, string projectName, string changes)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.ProjectUpdated,
                Description = $"Project '{projectName}' was updated",
                EntityType = "Project",
                EntityId = projectId,
                NewValues = changes
            });
        }

        public async Task LogTaskCreatedAsync(int projectId, int userId, string taskTitle)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.TaskCreated,
                Description = $"Task '{taskTitle}' was created",
                EntityType = "Task"
            });
        }

        public async Task LogTaskUpdatedAsync(int projectId, int userId, string taskTitle, string changes)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.TaskUpdated,
                Description = $"Task '{taskTitle}' was updated",
                EntityType = "Task",
                NewValues = changes
            });
        }

        public async Task LogTaskStatusChangedAsync(int projectId, int userId, string taskTitle, TaskStatus oldStatus, TaskStatus newStatus)
        {
            var oldValues = JsonSerializer.Serialize(new { Status = oldStatus.ToString() });
            var newValues = JsonSerializer.Serialize(new { Status = newStatus.ToString() });

            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.TaskStatusChanged,
                Description = $"Task '{taskTitle}' status changed from {oldStatus} to {newStatus}",
                EntityType = "Task",
                OldValues = oldValues,
                NewValues = newValues
            });
        }

        public async Task LogProjectMemberAddedAsync(int projectId, int userId, string memberName, ProjectMemberRole role)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.MemberAdded,
                Description = $"{memberName} was added to the project as {role}",
                EntityType = "ProjectMember"
            });
        }

        public async Task LogProjectMemberRemovedAsync(int projectId, int userId, string memberName)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.MemberRemoved,
                Description = $"{memberName} was removed from the project",
                EntityType = "ProjectMember"
            });
        }

        public async Task LogTaskAssignedAsync(int projectId, int userId, string taskTitle, string assigneeName)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.TaskAssigned,
                Description = $"Task '{taskTitle}' was assigned to {assigneeName}",
                EntityType = "Task"
            });
        }

        public async Task LogTaskCommentAddedAsync(int projectId, int userId, string taskTitle)
        {
            await LogActivityAsync(new LogActivityDto
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = ActivityType.CommentAdded,
                Description = $"Comment added to task '{taskTitle}'",
                EntityType = "TaskComment"
            });
        }
    }
}