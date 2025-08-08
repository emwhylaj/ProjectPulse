using Microsoft.EntityFrameworkCore.Storage;
using ProjectPulseAPI.Core.Persistence.Repository;
using ProjectPulseAPI.Infrastructure;

namespace ProjectPulseAPI.Core.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectPulseDbContext _context;
        private IDbContextTransaction? _transaction;

        private IUserRepository? _users;
        private IProjectRepository? _projects;
        private ITaskRepository? _tasks;
        private IProjectMemberRepository? _projectMembers;
        private ITaskCommentRepository? _taskComments;
        private INotificationRepository? _notifications;
        private IProjectActivityRepository? _projectActivities;

        public UnitOfWork(ProjectPulseDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _users ??= new UserRepository(_context);
        public IProjectRepository ProjectRepository => _projects ??= new ProjectRepository(_context);
        public ITaskRepository TaskRepository => _tasks ??= new TaskRepository(_context);
        public IProjectMemberRepository ProjectMemberRepository => _projectMembers ??= new ProjectMemberRepository(_context);
        public ITaskCommentRepository TaskCommentRepository => _taskComments ??= new TaskCommentRepository(_context);
        public INotificationRepository NotificationRepository => _notifications ??= new NotificationRepository(_context);
        public IProjectActivityRepository ProjectActivityRepository => _projectActivities ??= new ProjectActivityRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}