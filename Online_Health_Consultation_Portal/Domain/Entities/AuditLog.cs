namespace Online_Health_Consultation_Portal.Domain
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public int UserId { get; set; } // Admin thực hiện hành động
        public string ActionType { get; set; } // Thêm, Sửa, Xóa
        public string Entity { get; set; } // Tên thực thể (e.g., "Appointment", "Patient")
        public int EntityId { get; set; } // ID thực thể bị thay đổi
        public DateTime Timestamp { get; set; } // Thời gian thực hiện hành động

        public User User { get; set; }
    }
}
