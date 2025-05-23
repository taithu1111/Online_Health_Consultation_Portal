namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }

        public User User { get; set; }
    }
}
