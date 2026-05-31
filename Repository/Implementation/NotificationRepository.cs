namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
 
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
 
        public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotificationID == notificationId);
        }
 
        public async Task<List<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
 
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeleteNotificationAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
 
 