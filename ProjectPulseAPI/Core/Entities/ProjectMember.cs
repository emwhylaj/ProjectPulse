using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Entities
{
    public class ProjectMember : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Project Project { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}