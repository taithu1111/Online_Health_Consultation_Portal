using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginDto LoginDto { get; set; }
    }
}
