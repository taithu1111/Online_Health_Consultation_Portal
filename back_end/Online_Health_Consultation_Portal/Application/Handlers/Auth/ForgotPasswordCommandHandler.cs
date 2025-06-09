using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogService logService)
        {
            _userRepository = userRepository;   
            _emailService = emailService;
            _logService = logService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ForgotPasswordDto?.Email))
                {
                    await _logService.LogWarningAsync("Empty email provided", null, "ForgotPassword", "User", 0);
                    return false;
                }

                var user = await _userRepository.GetUserByEmailAsync(request.ForgotPasswordDto.Email);
                if (user == null)
                {
                    await _logService.LogWarningAsync(
                        $"User not found for email: {request.ForgotPasswordDto.Email}",
                        null, "ForgotPassword", "User", 0);
                    return false;
                }

                var resetToken = Guid.NewGuid().ToString();
                user.ResetPasswordToken = resetToken;
                user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);

                var updateSuccess = await _userRepository.UpdateUserProfileAsync(user);
                if (!updateSuccess)
                {
                    await _logService.LogErrorAsync(
                        "Failed to update user with reset token",
                        user.Id.ToString(), "ForgotPassword", "User", user.Id);
                    return false;
                }

                var emailSubject = "Reset Your Password";
                var emailBody = $@"<h2>Password Reset Request</h2>
                                <p>Please click the following link to reset your password:</p>
                                <a href='http://localhost:4200/authentication/reset-password?token={resetToken}'>
                                Reset Password</a>
                                <p>This link will expire in 1 hour.</p>";

                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
                return true;
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(
                    $"Unexpected error in ForgotPassword handler: {ex}",
                    null, "ForgotPassword", "User", 0);
                Console.WriteLine(ex + " ; Failed in ForgotPasswordCommand Handler.");
                return false;
            }
        }
    }
}
