using MediatR;
using System.Security.Claims;

namespace Online_Health_Consultation_Portal.Application.Commands.Users
{
    public class UpdateUserProfileCommand : IRequest<Unit>
    {
        public ClaimsPrincipal User { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        
        // Patient specific fields
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        
        // Doctor specific fields
        public string Bio { get; set; }
        public string Languages { get; set; }
    }
}