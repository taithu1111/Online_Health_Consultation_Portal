using FluentValidation;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.Application.Validator
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character.");
        }
    }
}
