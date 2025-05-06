using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Online_Health_Consultation_Portal.API.DTo;
using Online_Health_Consultation_Portal.Application.CQRS.Command;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;
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
            int currentUserId = 1; // Hardcoded for testing
            var query = new GetMessagesByConversationIdQuery
            {
                ConversationId = currentUserId,
                SenderId = currentUserId,
                ReceiverId = otherUserId,
                OtherUserId = otherUserId
            };

            var messages = await _mediator.Send(query);
            if (messages == null || !messages.Any())
            {
                return Ok(new List<Message>());
            }

            foreach (var message in messages.Where(m => !m.IsRead && m.SenderId == otherUserId))
            {
                Console.WriteLine($"Marking message {message.Id} as read for user {currentUserId}");
                await _mediator.Send(new MarkMessageAsReadCommand(message.Id, currentUserId));
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
            // Validate DTO
            if (messageDto == null || string.IsNullOrEmpty(messageDto.Content) || messageDto.ReceiverId <= 0)
            {
                return BadRequest("Invalid message data");
            }

            int currentUserId = 1; // Hardcoded for testing
            var command = new SendMessageCommand(currentUserId, messageDto.ReceiverId, messageDto.Content);

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
            int currentUserId = 1; // Hardcoded for testing
            var command = new MarkMessageAsReadCommand(id, currentUserId);

            await _mediator.Send(command);

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
