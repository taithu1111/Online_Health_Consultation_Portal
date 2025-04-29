namespace Online_Health_Consultation_Portal.Application.Dtos.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        // public DateTime CreatedDate { get; set; }
    }
}