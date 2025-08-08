using ProjectPulseAPI.Core.Persistence.Repository;

namespace ProjectPulseAPI.Core.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ITaskCommentRepository TaskCommentRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IProjectActivityRepository ProjectActivityRepository { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}