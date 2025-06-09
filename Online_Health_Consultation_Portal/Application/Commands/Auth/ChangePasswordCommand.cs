using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public ChangePasswordDto ChangePasswordDto { get; set; } = default!;
        public int UserId { get; set; } // Extracted from token
    }
}
