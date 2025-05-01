namespace Online_Health_Consultation_Portal.Domain
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? Message { get; set; } // Tin nhắn kèm theo nếu có
        public bool IsRead { get; set; } // Đã đọc
        public DateTime CreatedAt { get; set; } // Thời gian thông báo
        public NotificationType Type { get; set; } // Loại thông báo (ví dụ: Appointment, Prescription, Payment, System, etc.)
        public int? AppointmentId { get; set; }
        public int? PrescriptionId { get; set; }
        public int? PaymentId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Appointment? Appointment { get; set; }
        public Prescription? Prescription { get; set; }
        public Payment? Payment { get; set; }
    }
}
