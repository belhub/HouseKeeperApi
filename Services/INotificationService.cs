using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface INotificationService
    {
        Task<int> CreateNotification(NotificationDto notificationDto);
        Task<NotificationDto> GetNotificationsForUserId(int userId);
        Task<bool> UpdateNotification(NotificationDto notificationDto);
    }
}