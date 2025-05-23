using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth
{
    public class RegisterCommand : IRequest<bool> // trả về true nếu thành công
    {
        public RegisterDto RegisterDto { get; set; }

        public RegisterCommand(RegisterDto registerDto)
        {
            RegisterDto = registerDto;
        }

    }
}
