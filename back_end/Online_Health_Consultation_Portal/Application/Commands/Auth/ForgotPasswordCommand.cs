using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public ForgotPasswordDto ForgotPasswordDto { get; set; }
    }
}
