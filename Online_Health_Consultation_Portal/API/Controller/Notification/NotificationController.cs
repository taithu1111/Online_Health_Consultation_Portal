using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Notifications;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;
using Online_Health_Consultation_Portal.Application.Queries.Notifications;

namespace Online_Health_Consultation_Portal.Controllers.Notification
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto notificationDto)
        {
            var command = new CreateNotificationCommand(notificationDto);
            var notificationId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserNotifications), new { userId = notificationDto.UserId }, new { id = notificationId });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var query = new GetUserNotificationsQuery { UserId = userId };
            var notifications = await _mediator.Send(query);
            return Ok(notifications);
        }
    }
}