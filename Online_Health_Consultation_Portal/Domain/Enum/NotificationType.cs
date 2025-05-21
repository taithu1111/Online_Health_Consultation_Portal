namespace Online_Health_Consultation_Portal.Domain.Enum
{
    public enum NotificationType
    {
        System = 0,         // Thông báo hệ thống (ví dụ: chào mừng, cập nhật hệ thống)
        Appointment = 1,    // Lịch hẹn
        Prescription = 2,   // Đơn thuốc
        Payment = 3,        // Thanh toán
        Message = 4         // Tin nhắn
    }
}