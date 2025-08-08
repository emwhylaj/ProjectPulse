using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Entities
{
    // ProjectPulse.Core/Entities/Task.cs
    public class UserTask : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public Shared.Enums.TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
        public int ActualHours { get; set; }
        public int ProjectId { get; set; }
        public string Tags { get; set; } = string.Empty;
        public int AssignedToId { get; set; }
        public int? ParentTaskId { get; set; }
        public ICollection<TaskFile>? Files { get; set; } = new List<TaskFile>();
        public int Progress { get; set; }
        public Project Project { get; set; } = null!;

        public User AssignedTo { get; set; } = null!;
        public UserTask? ParentTask { get; set; }
        public ICollection<UserTask> SubTasks { get; set; } = new List<UserTask>();
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}