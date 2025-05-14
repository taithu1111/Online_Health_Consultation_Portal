using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;

namespace Online_Health_Consultation_Portal.Application.Commands.Notifications
{
    public class CreateNotificationCommand : IRequest<int>
    {
        public CreateNotificationDto NotificationDto { get; }

        public CreateNotificationCommand(CreateNotificationDto notificationDto)
        {
            NotificationDto = notificationDto;
        }
    }
}