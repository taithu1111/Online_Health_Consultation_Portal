using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;

namespace Online_Health_Consultation_Portal.Application.Queries.Notification
{
    public class GetUserNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
    {
        public int UserId { get; set; }
    }
}