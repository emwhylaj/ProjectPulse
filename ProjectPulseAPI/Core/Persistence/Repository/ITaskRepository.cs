using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface ITaskRepository : IRepository<UserTask>
    {
        Task<UserTask?> GetTaskWithDetailsAsync(int id);

        Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId, TaskStatus? status = null);

        Task<IEnumerable<UserTask>> GetProjectTasksAsync(int projectId);

        Task<IEnumerable<UserTask>> GetOverdueTasksAsync();

        Task<IEnumerable<UserTask>> GetTasksByStatusAsync(TaskStatus status);

        Task<IEnumerable<UserTask>> GetSubTasksAsync(int parentTaskId);

        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(int? projectId = null);

        Task<IEnumerable<UserTask>> GetTasksDueSoonAsync(int days = 3);

        Task<IEnumerable<UserTask>> SearchTasksAsync(string searchTerm, int? projectId = null, int? userId = null);
    }
}