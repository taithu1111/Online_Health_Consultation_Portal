using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Application.Dtos.Notifications
{
    public class CreateNotificationDto
    {
        [Required]
        public int UserId { get; set; }
        
        [StringLength(500)]
        public string? Message { get; set; }
        
        [Required]
        public NotificationType Type { get; set; }
        
        public int? AppointmentId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? PaymentId { get; set; }
    }
}