using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Files { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;

        public DateTime LastLoginAt { get; set; }

        // Navigation Properties
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

        public ICollection<UserTask> AssignedTasks { get; set; } = new List<UserTask>();
        public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
    }
}