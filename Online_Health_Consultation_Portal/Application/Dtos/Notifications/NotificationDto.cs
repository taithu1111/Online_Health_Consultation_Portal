namespace Online_Health_Consultation_Portal.Application.Dtos.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public int? AppointmentId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? PaymentId { get; set; }
    }
}