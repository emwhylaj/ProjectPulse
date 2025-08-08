using Microsoft.EntityFrameworkCore;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Infrastructure;
using ProjectPulseAPI.Shared.Enums;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ProjectPulseDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetProjectMembersAsync(int projectId)
        {
            return await _context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId && pm.IsActive)
                .Select(pm => pm.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _dbSet.Where(u => u.Role == role && u.IsActive).ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            var query = _dbSet.Where(u => u.Email.ToLower() == email.ToLower());
            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.Id != excludeUserId.Value);
            }
            return !await query.AnyAsync();
        }

        public async Task UpdateLastLoginAsync(int userId, DateTime loginTime)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = loginTime;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _dbSet
                .Where(u => u.IsActive &&
                    (u.FirstName.ToLower().Contains(term) ||
                     u.LastName.ToLower().Contains(term) ||
                     u.Email.ToLower().Contains(term)))
                .ToListAsync();
        }
    }

    public class ProjectRepository(ProjectPulseDbContext context) : Repository<Project>(context), IProjectRepository
    {
        public async Task<Project?> GetProjectWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProjectManager)
                .Include(p => p.Members).ThenInclude(m => m.User)
                .Include(p => p.Tasks).ThenInclude(t => t.AssignedTo)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
        {
            return await _context.ProjectMembers
                .Where(pm => pm.UserId == userId && pm.IsActive)
                .Select(pm => pm.Project)
                .Include(p => p.ProjectManager)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
        {
            return await _dbSet
                .Where(p => p.Status == status)
                .Include(p => p.ProjectManager)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId)
        {
            return await _dbSet
                .Where(p => p.ProjectManagerId == managerId)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetOverdueProjectsAsync()
        {
            return await _dbSet
                .Where(p => p.EndDate < DateTime.UtcNow &&
                           p.Status != ProjectStatus.Completed &&
                           p.Status != ProjectStatus.Cancelled)
                .Include(p => p.ProjectManager)
                .ToListAsync();
        }

        public async Task<Dictionary<ProjectStatus, int>> GetProjectStatusCountsAsync()
        {
            return await _dbSet
                .GroupBy(p => p.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm, int? userId = null)
        {
            var query = _dbSet.AsQueryable();

            if (userId.HasValue)
            {
                var projectIds = await _context.ProjectMembers
                    .Where(pm => pm.UserId == userId.Value && pm.IsActive)
                    .Select(pm => pm.ProjectId)
                    .ToListAsync();

                query = query.Where(p => projectIds.Contains(p.Id));
            }

            var term = searchTerm.ToLower();
            return await query
                .Where(p => p.Name.ToLower().Contains(term) ||
                           p.Description.ToLower().Contains(term))
                .Include(p => p.ProjectManager)
                .ToListAsync();
        }
    }

    public class TaskRepository(ProjectPulseDbContext context) : Repository<UserTask>(context), ITaskRepository
    {
        public async Task<UserTask?> GetTaskWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .Include(t => t.ParentTask)
                .Include(t => t.SubTasks)
                .Include(t => t.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId, TaskStatus? status = null)
        {
            var query = _dbSet.Where(t => t.AssignedToId == userId);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query
                .Include(t => t.Project)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetProjectTasksAsync(int projectId)
        {
            return await _dbSet
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedTo)
                .Include(t => t.ParentTask)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetOverdueTasksAsync()
        {
            return await _dbSet
                .Where(t => t.DueDate < DateTime.UtcNow &&
                           t.Status != TaskStatus.Completed)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetTasksByStatusAsync(TaskStatus status)
        {
            return await _dbSet
                .Where(t => t.Status == status)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetSubTasksAsync(int parentTaskId)
        {
            return await _dbSet
                .Where(t => t.ParentTaskId == parentTaskId)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(int? projectId = null)
        {
            var query = _dbSet.AsQueryable();

            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            return await query
                .GroupBy(t => t.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<UserTask>> GetTasksDueSoonAsync(int days = 3)
        {
            var dueDate = DateTime.UtcNow.AddDays(days);
            return await _dbSet
                .Where(t => t.DueDate <= dueDate &&
                           t.Status != TaskStatus.Completed)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> SearchTasksAsync(string searchTerm, int? projectId = null, int? userId = null)
        {
            var query = _dbSet.AsQueryable();

            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(t => t.AssignedToId == userId.Value);
            }

            var term = searchTerm.ToLower();
            return await query
                .Where(t => t.Title.ToLower().Contains(term) ||
                           t.Description.ToLower().Contains(term) ||
                           (t.Tags != null && t.Tags.ToLower().Contains(term)))
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }
    }

    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ProjectPulseDbContext context) : base(context)
        {
        }

        public async Task<ProjectMember?> GetMemberAsync(int projectId, int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
        }

        public async Task<ProjectMember?> GetProjectMemberAsync(int projectId, int userId)
        {
            return await GetMemberAsync(projectId, userId);
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId)
        {
            return await _dbSet
                .Where(pm => pm.ProjectId == projectId)
                .Include(pm => pm.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetActiveProjectMembersAsync(int projectId)
        {
            return await _dbSet
                .Where(pm => pm.ProjectId == projectId && pm.IsActive)
                .Include(pm => pm.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetUserMembershipsAsync(int userId)
        {
            return await _dbSet
                .Where(pm => pm.UserId == userId)
                .Include(pm => pm.Project)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetUserProjectMembershipsAsync(int userId)
        {
            return await GetUserMembershipsAsync(userId);
        }

        public async Task<bool> IsMemberAsync(int projectId, int userId)
        {
            return await _dbSet.AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.IsActive);
        }

        public async Task<bool> IsUserProjectMemberAsync(int projectId, int userId)
        {
            return await IsMemberAsync(projectId, userId);
        }

        public async Task<ProjectMemberRole?> GetMemberRoleAsync(int projectId, int userId)
        {
            var member = await _dbSet.FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.IsActive);
            return member?.Role;
        }

        public async Task RemoveMemberAsync(int projectId, int userId)
        {
            var member = await _dbSet.FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
            if (member != null)
            {
                member.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }

    public class TaskCommentRepository : Repository<TaskComment>, ITaskCommentRepository
    {
        public TaskCommentRepository(ProjectPulseDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _dbSet
                .Where(tc => tc.TaskId == taskId)
                .Include(tc => tc.User)
                .Include(tc => tc.Replies)
                .OrderBy(tc => tc.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskComment>> GetUserCommentsAsync(int userId)
        {
            return await _dbSet
                .Where(tc => tc.UserId == userId)
                .Include(tc => tc.Task)
                .OrderByDescending(tc => tc.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskComment?> GetCommentWithRepliesAsync(int commentId)
        {
            return await _dbSet
                .Where(tc => tc.Id == commentId)
                .Include(tc => tc.User)
                .Include(tc => tc.Replies)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskComment>> GetCommentRepliesAsync(int parentCommentId)
        {
            return await _dbSet
                .Where(tc => tc.ParentCommentId == parentCommentId)
                .Include(tc => tc.User)
                .OrderBy(tc => tc.CreatedAt)
                .ToListAsync();
        }
    }

    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ProjectPulseDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            var query = _dbSet.Where(n => n.UserId == userId);
            
            if (unreadOnly)
                query = query.Where(n => !n.IsRead);

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _dbSet.CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _dbSet.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task DeleteOldNotificationsAsync(DateTime beforeDate)
        {
            var oldNotifications = await _dbSet
                .Where(n => n.CreatedAt < beforeDate)
                .ToListAsync();

            _dbSet.RemoveRange(oldNotifications);
            await _context.SaveChangesAsync();
        }
    }

    public class ProjectActivityRepository : Repository<ProjectActivity>, IProjectActivityRepository
    {
        public ProjectActivityRepository(ProjectPulseDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProjectActivity>> GetProjectActivitiesAsync(int projectId, int limit = 50)
        {
            return await _dbSet
                .Where(pa => pa.ProjectId == projectId)
                .Include(pa => pa.User)
                .OrderByDescending(pa => pa.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectActivity>> GetUserActivitiesAsync(int userId, int limit = 50)
        {
            return await _dbSet
                .Where(pa => pa.UserId == userId)
                .Include(pa => pa.Project)
                .OrderByDescending(pa => pa.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectActivity>> GetActivitiesByTypeAsync(int projectId, ActivityType activityType)
        {
            return await _dbSet
                .Where(pa => pa.ProjectId == projectId && pa.ActivityType == activityType)
                .Include(pa => pa.User)
                .OrderByDescending(pa => pa.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectActivity>> GetRecentActivitiesAsync(int days = 7, int limit = 100)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            
            return await _dbSet
                .Where(pa => pa.CreatedAt >= cutoffDate)
                .Include(pa => pa.User)
                .Include(pa => pa.Project)
                .OrderByDescending(pa => pa.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task LogActivityAsync(int projectId, int userId, ActivityType activityType, string description, string? entityType = null, int? entityId = null, string? oldValues = null, string? newValues = null)
        {
            var activity = new ProjectActivity
            {
                ProjectId = projectId,
                UserId = userId,
                ActivityType = activityType,
                Description = description,
                EntityType = entityType,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues,
                CreatedBy = userId.ToString(),
                UpdatedBy = userId.ToString()
            };

            await _dbSet.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOldActivitiesAsync(DateTime beforeDate)
        {
            var oldActivities = await _dbSet
                .Where(pa => pa.CreatedAt < beforeDate)
                .ToListAsync();

            _dbSet.RemoveRange(oldActivities);
            await _context.SaveChangesAsync();
        }
    }
}