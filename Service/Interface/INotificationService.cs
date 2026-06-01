namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;
 
    public interface INotificationService
    {
        Task<List<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<List<NotificationResponseDto>> GetNotificationsForUserAsync(int userId);
        Task SendNotificationAsync(NotificationSendDto notificationDto);
        Task DeleteNotificationAsync(int notificationId);
        Task MarkAsReadAsync(int notificationId);
    }
}
 
 