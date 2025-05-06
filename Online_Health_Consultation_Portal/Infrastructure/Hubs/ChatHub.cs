using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        // Map user IDs to connection IDs
        private static readonly Dictionary<int, string> UserConnectionMap = new Dictionary<int, string>();


        public static bool IsUserConnected(int userId)
        {
            return UserConnectionMap.ContainsKey(userId);
        }

        // Connect a user to the hub
        public async Task ConnectUser(int userId)
        {
            UserConnectionMap[userId] = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            
            // Log user connection
            var log = new Log
            {
                Message = $"User {userId} connected to chat hub",
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                UserId = userId.ToString(),
                Action = "Connect",
                Entity = "ChatHub",
                EntityId = 0
            };
            
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // Send a message to a specific user
        public async Task SendPrivateMessage(int senderId, int receiverId, string message)
        {
            // If the receiver is connected, send them the message in real-time
            if (UserConnectionMap.ContainsKey(receiverId))
            {
                await Clients.Client(UserConnectionMap[receiverId]).SendAsync("ReceiveMessage", senderId, message);
            }
            
            // Also send to the sender's client to update their UI
            await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
        }

        // Notify when a message is read
        public async Task MarkMessageAsRead(int messageId, int senderId)
        {
            if (UserConnectionMap.ContainsKey(senderId))
            {
                await Clients.Client(UserConnectionMap[senderId]).SendAsync("MessageRead", messageId);
            }
        }

        // Handle disconnection
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Find and remove the disconnected user
            var userId = UserConnectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != 0)
            {
                UserConnectionMap.Remove(userId);
                
                // Log user disconnection
                var log = new Log
                {
                    Message = $"User {userId} disconnected from chat hub",
                    Level = "Information",
                    Timestamp = DateTime.UtcNow,
                    UserId = userId.ToString(),
                    Action = "Disconnect",
                    Entity = "ChatHub",
                    EntityId = 0
                };
                
                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
