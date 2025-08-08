using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<UserTask>> GetAllTasksAsync();
        Task<UserTask?> GetTaskByIdAsync(int id);
        Task<UserTask?> GetTaskWithDetailsAsync(int id);
        Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId, TaskStatus? status = null);
        Task<IEnumerable<UserTask>> GetProjectTasksAsync(int projectId);
        Task<IEnumerable<UserTask>> GetOverdueTasksAsync();
        Task<IEnumerable<UserTask>> GetTasksByStatusAsync(TaskStatus status);
        Task<IEnumerable<UserTask>> GetSubTasksAsync(int parentTaskId);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(int? projectId = null);
        Task<IEnumerable<UserTask>> GetTasksDueSoonAsync(int days = 3);
        Task<IEnumerable<UserTask>> SearchTasksAsync(string searchTerm, int? projectId = null, int? userId = null);
        Task<UserTask> CreateTaskAsync(CreateTaskDto taskDto, int createdBy);
        Task<UserTask?> UpdateTaskAsync(int id, UpdateTaskDto taskDto, int updatedBy);
        Task<bool> DeleteTaskAsync(int id, int deletedBy);
        Task<bool> AssignTaskAsync(int taskId, int userId, int assignedBy);
        Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatus status, int updatedBy);
        Task<bool> UpdateTaskProgressAsync(int taskId, int progress, int updatedBy);
        Task<TaskComment> AddTaskCommentAsync(int taskId, string content, int userId);
        Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId);
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
        public int ProjectId { get; set; }
        public int AssignedToId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Tags { get; set; } = string.Empty;
    }

    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public TaskStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public int? AssignedToId { get; set; }
        public int? ParentTaskId { get; set; }
        public string? Tags { get; set; }
        public int? Progress { get; set; }
    }
}