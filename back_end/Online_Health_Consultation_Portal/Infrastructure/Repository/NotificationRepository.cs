using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext context) : INotificationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Notification> CreateAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Id)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.Notifications.Update(notification);
            }
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}