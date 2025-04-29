using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;
using Online_Health_Consultation_Portal.Infrastructure.Hubs;


namespace Online_Health_Consultation_Portal.Infrastructure.Repository;

public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageRepository(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<List<Message>> GetAllAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                    (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<Message>> GetMessagesByUserAsync(int userId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<Message>> GetUnreadMessagesForUserAsync(int userId)
        {
            return await _context.Messages
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Message message)
        {
            message.SentAt = DateTime.UtcNow;
            message.IsRead = false;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            // Log the message creation
            var log = new Log
            {
                Message = $"Message sent from user {message.SenderId} to user {message.ReceiverId}",
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                UserId = message.SenderId.ToString(),
                Action = "Create",
                Entity = "Message",
                EntityId = message.Id
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Groups($"User_{message.SenderId}").SendAsync("ReceiveMessage", message.SenderId
            ,message.Content,message.Id,message.SentAt);

            return message.Id;
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();

            // Log the message update
            var log = new Log
            {
                Message = $"Message {message.Id} updated",
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                UserId = message.SenderId.ToString(),
                Action = "Update",
                Entity = "Message",
                EntityId = message.Id
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();

                // Log the message deletion
                var log = new Log
                {
                    Message = $"Message {id} deleted",
                    Level = "Information",
                    Timestamp = DateTime.UtcNow,
                    UserId = message.SenderId.ToString(),
                    Action = "Delete",
                    Entity = "Message",
                    EntityId = id
                };

                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message is { IsRead: false })
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;

                _context.Messages.Update(message);
                await _context.SaveChangesAsync();

                // Log the message being marked as read
                var log = new Log
                {
                    Message = $"Message {messageId} marked as read",
                    Level = "Information",
                    Timestamp = DateTime.UtcNow,
                    UserId = message.ReceiverId.ToString(),
                    Action = "Update",
                    Entity = "Message",
                    EntityId = messageId
                };

                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Groups($"User_{message.SenderId}").SendAsync("MessageRead", messageId);
            }
        }
    }