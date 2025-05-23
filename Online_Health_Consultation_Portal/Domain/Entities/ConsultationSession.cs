using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class ConsultationSession
    {
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; }  // Gắn liền với lịch hẹn

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string SessionNotes { get; set; } // Ghi chú về buổi khám (nếu có)

        public string MeetingUrl { get; set; } // Link video call hoặc chat room

        // Navigation
        public Appointment Appointment { get; set; }
    }
}
