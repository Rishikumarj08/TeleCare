namespace TeleCare.Service.Implementation
{
    using TeleCare.Constants;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;
 
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
 
        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }
 
        public async Task<List<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync();
            return notifications.Select(Map).ToList();
        }
 
        public async Task<List<NotificationResponseDto>> GetNotificationsForUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            if (notifications == null || notifications.Count == 0)
                throw new NotFoundException(AppConstants.NoNotificationsFound);
            return notifications.Select(Map).ToList();
        }
 
        public async Task SendNotificationAsync(NotificationSendDto notificationDto)
        {
            var user = await _userRepository.GetUserByIdAsync(notificationDto.UserID);
            if (user == null)
                throw new NotFoundException(AppConstants.RecipientUserNotFound);
 
            var notification = new Notification
            {
                UserID = notificationDto.UserID,
                Message = notificationDto.Message,
                Category = notificationDto.Category,
                Status = "Unread",
                CreatedAt = DateTime.UtcNow
            };
 
            await _notificationRepository.AddNotificationAsync(notification);
        }
 
        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(AppConstants.NotificationNotFound);
            await _notificationRepository.DeleteNotificationAsync(notification);
        }
 
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(AppConstants.NotificationNotFound);
 
            notification.Status = "Read";
            await _notificationRepository.UpdateNotificationAsync(notification);
        }
 
        private static NotificationResponseDto Map(Notification notification) => new()
        {
            NotificationID = notification.NotificationID,
            UserID = notification.UserID,
            RecipientName = notification.User?.Name ?? string.Empty,
            Message = notification.Message,
            Category = notification.Category,
            Status = notification.Status,
            CreatedAt = notification.CreatedAt
        };
    }
}
 
 