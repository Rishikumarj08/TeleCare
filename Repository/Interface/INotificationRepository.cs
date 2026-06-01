namespace TeleCare.Repository.Interface
{
    using TeleCare.Model;
 
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Notification notification);
    }
}
 
 