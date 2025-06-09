using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public ResetPasswordDto ResetPasswordDto { get; set; }
    }
}
