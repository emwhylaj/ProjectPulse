namespace ProjectPulseAPI.Core.Entities
{
    public class TaskComment : BaseEntity
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }

        // Navigation Properties
        public UserTask Task { get; set; } = null!;

        public User User { get; set; } = null!;
        public TaskComment? ParentComment { get; set; }
        public ICollection<TaskComment> Replies { get; set; } = new List<TaskComment>();
    }
}