using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetProjectWithDetailsAsync(int id);

        Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);

        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);

        Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId);

        Task<IEnumerable<Project>> GetOverdueProjectsAsync();

        Task<Dictionary<ProjectStatus, int>> GetProjectStatusCountsAsync();

        Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm, int? userId = null);
    }
}