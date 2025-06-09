// CreateNotificationCommandHandler.cs
using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Notifications;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;

namespace Online_Health_Consultation_Portal.Application.Handlers.Notifications
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = request.NotificationDto.UserId,
                Message = request.NotificationDto.Message,
                IsRead = false,
                Type = request.NotificationDto.Type,
                AppointmentId = request.NotificationDto.AppointmentId,
                PrescriptionId = request.NotificationDto.PrescriptionId,
                PaymentId = request.NotificationDto.PaymentId,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.CreateAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return notification.Id;
        }
    }
}