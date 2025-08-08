using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Entities
{
    public class ProjectActivity : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        // Navigation Properties
        public Project Project { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}