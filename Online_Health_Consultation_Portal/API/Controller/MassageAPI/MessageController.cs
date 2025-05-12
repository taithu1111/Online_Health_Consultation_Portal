using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Online_Health_Consultation_Portal.Application.CQRS.Command;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;
using Online_Health_Consultation_Portal.Application.Dtos.Massage;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Hubs;

namespace Online_Health_Consultation_Portal.API.Controller.MassageAPI
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IMediator mediator, IHubContext<ChatHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get messages between the current user and another user
        /// </summary>
        /// <param name="otherUserId">The ID of the other user in the conversation</param>
        /// <returns>List of messages between the users</returns>
        [HttpGet("conversation/{otherUserId}")]
        public async Task<ActionResult<List<Message>>> GetMessagesByConversation(int otherUserId)
        {
            // Get the current user's ID from claims
            //dev mode
            //int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier);

            int currentUserId = 1;
            var query = new GetMessagesByConversationIdQuery
            {
                ConversationId = currentUserId,
                SenderId = currentUserId,
                ReceiverId = otherUserId,
                OtherUserId = otherUserId
            };

            var messages = await _mediator.Send(query);

            // Mark unread messages as read since the user is viewing them
            foreach (var message in messages.Where(m => !m.IsRead && m.SenderId == otherUserId))
            {
                await _mediator.Send(new MarkMessageAsReadCommand
                {
                    MessageId = message.Id,
                    UserId = currentUserId
                });
            }

            return Ok(messages);
        }

        /// <summary>
        /// Send a new message to another user
        /// </summary>
        /// <param name="messageDto">The message data</param>
        /// <returns>The ID of the created message</returns>
        [HttpPost]
        public async Task<ActionResult<int>> SendMessage([FromBody] SendMessageDto messageDto)
        {
            // Get the current user's ID from claims
            // var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
            int currentUserId = 1;
            var command = new SendMessageCommand
            {
                SenderId = currentUserId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content
            };

            var messageId = await _mediator.Send(command);

            // Notify the recipient via SignalR
            await _hubContext.Clients.Group($"User_{messageDto.ReceiverId}").SendAsync(
                "ReceiveMessage",
                currentUserId,
                messageDto.Content,
                messageId,
                DateTime.UtcNow
            );

            return Ok(messageId);
        }

        /// <summary>
        /// Mark a message as read
        /// </summary>
        /// <param name="id">The message ID</param>
        /// <returns>No content</returns>
        [HttpPut("{id}/read")]
        public async Task<ActionResult> MarkMessageAsRead(int id)
        {
            // Get the current user's ID from claims
            // var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int currentUserId = 1;//rn Just ttest wit user Id 1

            var command = new MarkMessageAsReadCommand
            {
                MessageId = id,
                UserId = currentUserId
            };

            await _mediator.Send(command);

            // Get the message to find the sender
            var message = await _mediator.Send(new GetMessageByIdQuery { Id = id });
            await _hubContext.Clients.Group($"User_{message.SenderId}").SendAsync(
                "MessageRead",
                id
            );

            return NoContent();
        }

        /// <summary>
        /// Get user connection status
        /// </summary>
        /// <param name="userId">The user ID to check</param>
        /// <returns>Connection status</returns>
        [HttpGet("user/{userId}/status")]
        public ActionResult<bool> GetUserConnectionStatus(int userId)
        {
            // This is a placeholder implementation
            // In a real implementation, you would need to track connected users in the ChatHub
            // and check if the user is in the connected users list

            // For now, we'll just return a placeholder value
            bool isConnected = ChatHub.IsUserConnected(userId);
            return Ok(isConnected);
        }
    }
}
