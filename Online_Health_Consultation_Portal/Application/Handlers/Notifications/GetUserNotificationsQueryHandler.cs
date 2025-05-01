using AutoMapper;
using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;
using Online_Health_Consultation_Portal.Application.Queries.Notifications;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;

namespace Online_Health_Consultation_Portal.Application.Handlers.Notifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(request.UserId);

            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }
    }
}