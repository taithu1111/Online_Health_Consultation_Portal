using System.Security.Claims;
using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;

namespace Online_Health_Consultation_Portal.Application.Commands.Users
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public ClaimsPrincipal User { get; set; }
        public ChangePasswordDto changePasswordDto { get; set; }
    }
}