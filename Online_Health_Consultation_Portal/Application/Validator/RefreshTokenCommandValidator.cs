using FluentValidation;
using Online_Health_Consultation_Portal.Application.Commands.Auth.RefreshToken;

namespace Online_Health_Consultation_Portal.Application.Validator
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.TokenDto.AccessToken).NotEmpty();
            RuleFor(x => x.TokenDto.RefreshToken).NotEmpty();
        }
    }
}
