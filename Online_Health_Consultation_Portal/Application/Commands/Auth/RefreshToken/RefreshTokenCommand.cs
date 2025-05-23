using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.RefreshToken;

namespace Online_Health_Consultation_Portal.Application.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<TokenResponseDto?>
    {
        public TokenDto TokenDto { get; set; }
    }
}
