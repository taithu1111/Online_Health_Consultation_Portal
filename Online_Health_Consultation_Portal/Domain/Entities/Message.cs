﻿namespace Online_Health_Consultation_Portal.Domain
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
