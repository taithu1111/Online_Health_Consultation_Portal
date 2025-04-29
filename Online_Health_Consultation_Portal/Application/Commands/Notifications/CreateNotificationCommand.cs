using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Application.Commands.Notifications
{
    public class CreateNotificationCommand : IRequest<int>
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
}