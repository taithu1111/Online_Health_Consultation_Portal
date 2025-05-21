namespace Online_Health_Consultation_Portal.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } // Nội dung log
        public string Level { get; set; } // Mức độ log (Info, Warning, Error)
        public DateTime Timestamp { get; set; } // Thời gian ghi log
        public string? UserId { get; set; } // ID của người dùng thực hiện thao tác, có thể trống nếu như thao tác thực hiện không tìm thấy người dùng do lỗi
        public string Action { get; set; } // Hành động (Login, Create, Update, Delete)
        public string Entity { get; set; } // Tên thực thể (Department, Employee, Shift, v.v.)
        public int EntityId { get; set; } // ID của thực thể bị ảnh hưởng

        public Log() { }
    }
}
