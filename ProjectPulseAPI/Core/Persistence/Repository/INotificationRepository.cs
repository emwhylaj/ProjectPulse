using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.GenericRepo;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Core.Persistence.Repository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);

        Task<int> GetUnreadCountAsync(int userId);

        Task MarkAsReadAsync(int notificationId);

        Task MarkAllAsReadAsync(int userId);

        Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, NotificationType type);

        Task DeleteOldNotificationsAsync(DateTime beforeDate);
    }
}