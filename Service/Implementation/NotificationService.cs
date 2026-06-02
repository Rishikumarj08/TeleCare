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
        private readonly IAuditLogService _auditLogService;

        public NotificationService(INotificationRepository notificationRepository,
            IUserRepository userRepository, IAuditLogService auditLogService)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync(notificationDto.UserID, "CREATE", "Notification",
                notification.NotificationID,
                $"Notification sent to '{user.Name}' with category '{notificationDto.Category}'.");
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(AppConstants.NotificationNotFound);

            var userId = notification.UserID;
            var userName = notification.User?.Name ?? string.Empty;
            await _notificationRepository.DeleteNotificationAsync(notification);
            await _auditLogService.LogAsync(userId, "DELETE", "Notification", notificationId,
                $"Notification '{notificationId}' deleted for user '{userName}'.");
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
