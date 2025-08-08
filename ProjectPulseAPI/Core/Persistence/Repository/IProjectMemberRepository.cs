using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        Task<ProjectMember?> GetMemberAsync(int projectId, int userId);
        Task<ProjectMember?> GetProjectMemberAsync(int projectId, int userId);

        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId);
        Task<IEnumerable<ProjectMember>> GetActiveProjectMembersAsync(int projectId);

        Task<IEnumerable<ProjectMember>> GetUserMembershipsAsync(int userId);
        Task<IEnumerable<ProjectMember>> GetUserProjectMembershipsAsync(int userId);

        Task<bool> IsMemberAsync(int projectId, int userId);
        Task<bool> IsUserProjectMemberAsync(int projectId, int userId);

        Task<ProjectMemberRole?> GetMemberRoleAsync(int projectId, int userId);

        Task RemoveMemberAsync(int projectId, int userId);
    }
}