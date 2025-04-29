using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;
using Online_Health_Consultation_Portal.Application.Queries.Notifications;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;

namespace Online_Health_Consultation_Portal.Application.Handlers.Notifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(request.UserId);
            
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                // CreatedDate = n.CreatedDate
            });
        }
    }
}