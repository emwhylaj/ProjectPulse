using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        Task<IEnumerable<User>> GetProjectMembersAsync(int projectId);

        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);

        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);

        System.Threading.Tasks.Task UpdateLastLoginAsync(int userId, DateTime loginTime);

        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
    }
}