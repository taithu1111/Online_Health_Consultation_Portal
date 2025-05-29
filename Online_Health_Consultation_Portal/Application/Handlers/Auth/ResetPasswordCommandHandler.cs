using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Service;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;

        public ResetPasswordCommandHandler(UserManager<User> userManager, ILogService logService)
        {
            _userManager = userManager;
            _logService = logService;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.ResetPasswordDto.Email);
            if (user == null)
            {
                await _logService.LogWarningAsync("Reset password failed: User not found.", null, "ResetPassword", "User", 0);
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordDto.Token, request.ResetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                await _logService.LogWarningAsync($"Reset password failed: {errors}", user.Id.ToString(), "ResetPassword", "User", user.Id);
                return false;
            }

            await _logService.LogInformationAsync("Password reset successful.", user.Id.ToString(), "ResetPassword", "User", user.Id);
            return true;
        }
    }
}
