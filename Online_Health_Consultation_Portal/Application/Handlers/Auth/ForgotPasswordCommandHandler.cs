using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;

        public ForgotPasswordCommandHandler(
            IRepository<User> userRepository,
            IEmailService emailService,
            ILogService logService)
        {
            _userRepository = userRepository;   
            _emailService = emailService;
            _logService = logService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var employees = await _userRepository.GetAllAsync();
            var employee = employees.FirstOrDefault(e => e.Email == request.ForgotPasswordDto.Email);

            if (employee == null)
            {
                await _logService.LogWarningAsync("Forgot password failed: Email not found.", null, "ForgotPassword",
                    "Employee", 0);
                return false;
            }

            // Tạo token reset password
            var resetToken = Guid.NewGuid().ToString();
            employee.ResetPasswordToken = resetToken;
            employee.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1); // Token hết hạn sau 1 giờ

            await _userRepository.UpdateAsync(employee);

            // Gửi email reset password
            var emailSubject = "Reset Your Password";
            var emailBody = $"Please reset your password using this token: {resetToken}";
            await _emailService.SendEmailAsync(employee.Email, emailSubject, emailBody);

            await _logService.LogInformationAsync("Reset password token sent.", employee.Id.ToString(), "ForgotPassword",
                "Employee", employee.Id);

            return true;
        }
    }
}
