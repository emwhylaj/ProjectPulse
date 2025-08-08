using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserTask>> GetAllTasksAsync()
        {
            return await _unitOfWork.TaskRepository.GetAllAsync();
        }

        public async Task<UserTask?> GetTaskByIdAsync(int id)
        {
            return await _unitOfWork.TaskRepository.GetByIdAsync(id);
        }

        public async Task<UserTask?> GetTaskWithDetailsAsync(int id)
        {
            return await _unitOfWork.TaskRepository.GetTaskWithDetailsAsync(id);
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId, TaskStatus? status = null)
        {
            return await _unitOfWork.TaskRepository.GetUserTasksAsync(userId, status);
        }

        public async Task<IEnumerable<UserTask>> GetProjectTasksAsync(int projectId)
        {
            return await _unitOfWork.TaskRepository.GetProjectTasksAsync(projectId);
        }

        public async Task<IEnumerable<UserTask>> GetOverdueTasksAsync()
        {
            return await _unitOfWork.TaskRepository.GetOverdueTasksAsync();
        }

        public async Task<IEnumerable<UserTask>> GetTasksByStatusAsync(TaskStatus status)
        {
            return await _unitOfWork.TaskRepository.GetTasksByStatusAsync(status);
        }

        public async Task<IEnumerable<UserTask>> GetSubTasksAsync(int parentTaskId)
        {
            return await _unitOfWork.TaskRepository.GetSubTasksAsync(parentTaskId);
        }

        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(int? projectId = null)
        {
            return await _unitOfWork.TaskRepository.GetTaskStatusCountsAsync(projectId);
        }

        public async Task<IEnumerable<UserTask>> GetTasksDueSoonAsync(int days = 3)
        {
            return await _unitOfWork.TaskRepository.GetTasksDueSoonAsync(days);
        }

        public async Task<IEnumerable<UserTask>> SearchTasksAsync(string searchTerm, int? projectId = null, int? userId = null)
        {
            return await _unitOfWork.TaskRepository.SearchTasksAsync(searchTerm, projectId, userId);
        }

        public async Task<UserTask> CreateTaskAsync(CreateTaskDto taskDto, int createdBy)
        {
            var task = new UserTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = taskDto.Priority,
                Status = TaskStatus.NotStarted,
                DueDate = taskDto.DueDate,
                EstimatedHours = taskDto.EstimatedHours,
                ProjectId = taskDto.ProjectId,
                AssignedToId = taskDto.AssignedToId,
                ParentTaskId = taskDto.ParentTaskId,
                Tags = taskDto.Tags,
                Progress = 0,
                ActualHours = 0,
                CreatedBy = createdBy.ToString(),
                UpdatedBy = createdBy.ToString()
            };

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return task;
        }

        public async Task<UserTask?> UpdateTaskAsync(int id, UpdateTaskDto taskDto, int updatedBy)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null) return null;

            if (!string.IsNullOrEmpty(taskDto.Title))
                task.Title = taskDto.Title;
            if (!string.IsNullOrEmpty(taskDto.Description))
                task.Description = taskDto.Description;
            if (taskDto.Priority.HasValue)
                task.Priority = taskDto.Priority.Value;
            if (taskDto.Status.HasValue)
                task.Status = taskDto.Status.Value;
            if (taskDto.DueDate.HasValue)
                task.DueDate = taskDto.DueDate.Value;
            if (taskDto.EstimatedHours.HasValue)
                task.EstimatedHours = taskDto.EstimatedHours.Value;
            if (taskDto.ActualHours.HasValue)
                task.ActualHours = taskDto.ActualHours.Value;
            if (taskDto.AssignedToId.HasValue)
                task.AssignedToId = taskDto.AssignedToId.Value;
            if (taskDto.ParentTaskId.HasValue)
                task.ParentTaskId = taskDto.ParentTaskId.Value;
            if (!string.IsNullOrEmpty(taskDto.Tags))
                task.Tags = taskDto.Tags;
            if (taskDto.Progress.HasValue)
                task.Progress = taskDto.Progress.Value;

            task.UpdatedBy = updatedBy.ToString();

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id, int deletedBy)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null) return false;

            task.IsDeleted = true;
            task.DeletedBy = deletedBy.ToString();
            task.UpdatedBy = deletedBy.ToString();

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignTaskAsync(int taskId, int userId, int assignedBy)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            task.AssignedToId = userId;
            task.UpdatedBy = assignedBy.ToString();

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatus status, int updatedBy)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = status;
            task.UpdatedBy = updatedBy.ToString();

            // Auto-update progress based on status
            task.Progress = status switch
            {
                TaskStatus.NotStarted => 0,
                TaskStatus.InProgress => task.Progress > 0 ? task.Progress : 25,
                TaskStatus.Review => 90,
                TaskStatus.Completed => 100,
                TaskStatus.Cancelled => task.Progress,
                TaskStatus.OnHold => task.Progress,
                _ => task.Progress
            };

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTaskProgressAsync(int taskId, int progress, int updatedBy)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Progress = Math.Clamp(progress, 0, 100);
            task.UpdatedBy = updatedBy.ToString();

            // Auto-update status based on progress
            if (progress == 0)
                task.Status = TaskStatus.NotStarted;
            else if (progress == 100)
                task.Status = TaskStatus.Completed;
            else if (progress >= 90)
                task.Status = TaskStatus.Review;
            else if (progress > 0)
                task.Status = TaskStatus.InProgress;

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<TaskComment> AddTaskCommentAsync(int taskId, string content, int userId)
        {
            var comment = new TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                Content = content,
                CreatedBy = userId.ToString(),
                UpdatedBy = userId.ToString()
            };

            await _unitOfWork.TaskCommentRepository.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            return comment;
        }

        public async Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _unitOfWork.TaskCommentRepository.GetTaskCommentsAsync(taskId);
        }
    }
}