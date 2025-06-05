using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Application.Dtos.Message
{
    public class SendMessageDto
    {
        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
