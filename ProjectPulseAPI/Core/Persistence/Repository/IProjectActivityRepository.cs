using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface IProjectActivityRepository : IRepository<ProjectActivity>
    {
        Task<IEnumerable<ProjectActivity>> GetProjectActivitiesAsync(int projectId, int limit = 50);
        Task<IEnumerable<ProjectActivity>> GetUserActivitiesAsync(int userId, int limit = 50);
        Task<IEnumerable<ProjectActivity>> GetActivitiesByTypeAsync(int projectId, ActivityType activityType);
        Task<IEnumerable<ProjectActivity>> GetRecentActivitiesAsync(int days = 7, int limit = 100);
        Task LogActivityAsync(int projectId, int userId, ActivityType activityType, string description, string? entityType = null, int? entityId = null, string? oldValues = null, string? newValues = null);
        Task DeleteOldActivitiesAsync(DateTime beforeDate);
    }
}