using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Entities
{
    // ProjectPulse.Core/Entities/Project.cs
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public decimal Budget { get; set; }
        public decimal? ActualCost { get; set; }
        public string? Color { get; set; }
        public int ProjectManagerId { get; set; }
        public IEnumerable<ProjectFile>? Files { get; set; }
        public ProjectPriority? Priority { get; set; }

        // Navigation Properties
        public User ProjectManager { get; set; } = null!;

        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public IEnumerable<ProjectActivity>? Activities { get; internal set; }
    }
}