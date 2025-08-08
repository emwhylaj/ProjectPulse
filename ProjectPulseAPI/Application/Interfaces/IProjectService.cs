using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<Project?> GetProjectWithDetailsAsync(int id);
        Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);
        Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId);
        Task<IEnumerable<Project>> GetOverdueProjectsAsync();
        Task<Dictionary<ProjectStatus, int>> GetProjectStatusCountsAsync();
        Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm, int? userId = null);
        Task<Project> CreateProjectAsync(CreateProjectDto projectDto, int createdBy);
        Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto projectDto, int updatedBy);
        Task<bool> DeleteProjectAsync(int id, int deletedBy);
        Task<bool> AddProjectMemberAsync(int projectId, int userId, ProjectMemberRole role, int addedBy);
        Task<bool> RemoveProjectMemberAsync(int projectId, int userId, int removedBy);
        Task<bool> UpdateProjectMemberRoleAsync(int projectId, int userId, ProjectMemberRole newRole, int updatedBy);
        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId);
    }

    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public ProjectPriority? Priority { get; set; }
        public string? Color { get; set; }
        public int ProjectManagerId { get; set; }
    }

    public class UpdateProjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus? Status { get; set; }
        public decimal? Budget { get; set; }
        public decimal? ActualCost { get; set; }
        public ProjectPriority? Priority { get; set; }
        public string? Color { get; set; }
        public int? ProjectManagerId { get; set; }
    }
}