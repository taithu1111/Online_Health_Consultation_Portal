namespace Online_Health_Consultation_Portal.Application.Dtos.Message
{
    public class MessageDto
    {
        public int Id { get; set; }

        // Id người gửi (int)
        public int SenderId { get; set; }

        // Id người nhận (int)
        public int ReceiverId { get; set; }

        // Nội dung tin nhắn
        public string Content { get; set; }

        // Thời gian gửi
        public DateTime SentAt { get; set; }

        // Trạng thái đã xem chưa
        public bool IsRead { get; set; }

        // Nếu đã xem, thời gian xem
        public DateTime? ReadAt { get; set; }
    }
}
