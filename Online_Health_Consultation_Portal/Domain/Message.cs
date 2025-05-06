using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Online_Health_Consultation_Portal.Domain.Enum;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

        // Optional - for tracking message status
        public MessageStatus Status { get; set; } = MessageStatus.Sent;

        public Message()
        {

        }

        public Message(int id, int senderId, int receiverId, string content, DateTime sentAt, bool isRead, DateTime? readAt, User sender, User receiver, MessageStatus status)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            SentAt = sentAt;
            IsRead = isRead;
            ReadAt = readAt;
            Sender = sender;
            Receiver = receiver;
            Status = status;
        }
    }


}