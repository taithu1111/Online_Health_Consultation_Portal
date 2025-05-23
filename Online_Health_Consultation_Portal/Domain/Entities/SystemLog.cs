namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class SystemLog
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public string LogLevel { get; set; } // Info, Warning, Error
        public string Message { get; set; } // Mô tả lỗi
        public DateTime Timestamp { get; set; } // Thời gian lỗi xảy ra
        public string UserId { get; set; } // ID người dùng thực hiện hành động
    }
}
