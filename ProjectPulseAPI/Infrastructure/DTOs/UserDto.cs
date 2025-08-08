using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Infrastructure.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    //public class UpdateUserDto
    //{
    //    public string FirstName { get; set; } = string.Empty;
    //    public string LastName { get; set; } = string.Empty;
    //    public string Email { get; set; } = string.Empty;
    //    public UserRole Role { get; set; }
    //    public bool IsActive { get; set; }
    //    public string? PhoneNumber { get; set; }
    //}

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = null!;
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
        public decimal Budget { get; set; }
        public decimal ActualCost { get; set; }
        public string? Color { get; set; }
        public int ProjectManagerId { get; set; }
        public UserDto? ProjectManager { get; set; }
        public List<ProjectMemberDto> Members { get; set; } = new();
        public List<TaskDto> Tasks { get; set; } = new();
        public ProjectStatsDto? Stats { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
        public decimal Budget { get; set; }
        public string? Color { get; set; }
        public int ProjectManagerId { get; set; }
    }

    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
        public decimal Budget { get; set; }
        public decimal ActualCost { get; set; }
        public string? Color { get; set; }
        public int ProjectManagerId { get; set; }
    }

    public class ProjectStatsDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int TotalMembers { get; set; }
        public int DaysRemaining { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class ProjectMemberDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsActive { get; set; }
        public UserDto User { get; set; } = null!;
    }

    public class AddProjectMemberDto
    {
        public int UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
        public int ActualHours { get; set; }
        public decimal Progress { get; set; }
        public string? Tags { get; set; }
        public int ProjectId { get; set; }
        public int AssignedToId { get; set; }
        public int? ParentTaskId { get; set; }

        public ProjectDto? Project { get; set; }
        public UserDto? AssignedTo { get; set; }
        public TaskDto? ParentTask { get; set; }
        public List<TaskDto> SubTasks { get; set; } = new();
        public List<TaskCommentDto> Comments { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsOverdue => DueDate < DateTime.UtcNow && Status != TaskStatus.Completed;
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
        public string? Tags { get; set; }
        public int ProjectId { get; set; }
        public int AssignedToId { get; set; }
        public int? ParentTaskId { get; set; }
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
        public int ActualHours { get; set; }
        public decimal Progress { get; set; }
        public string? Tags { get; set; }
        public int AssignedToId { get; set; }
    }

    public class UpdateTaskStatusDto
    {
        public TaskStatus Status { get; set; }
        public decimal Progress { get; set; }
        public int? ActualHours { get; set; }
    }

    public class TaskCommentDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDto User { get; set; } = null!;
        public List<TaskCommentDto> Replies { get; set; } = new();
    }

    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public string? ActionUrl { get; set; }
        public int? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
    }

    public class NotificationStatsDto
    {
        public int TotalCount { get; set; }
        public int UnreadCount { get; set; }
    }

    public class DashboardStatsDto
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalTasks { get; set; }
        public int MyTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int TasksDueToday { get; set; }
        public int TeamMembers { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class ProjectActivityDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ActivityType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTaskCommentDto
    {
        public int TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}