using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface ITaskCommentRepository : IRepository<TaskComment>
    {
        Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<IEnumerable<TaskComment>> GetUserCommentsAsync(int userId);
        Task<TaskComment?> GetCommentWithRepliesAsync(int commentId);
        Task<IEnumerable<TaskComment>> GetCommentRepliesAsync(int parentCommentId);
    }
}