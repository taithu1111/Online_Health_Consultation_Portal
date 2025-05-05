using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using System.Text;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogService _logService;

        public ResetPasswordCommandHandler(IRepository<User> userRepository, ILogService logService)
        {
            _userRepository = userRepository;
            _logService = logService;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var employees = await _userRepository.GetAllAsync();
            var employee = employees.FirstOrDefault(e => e.Email == request.ResetPasswordDto.Email &&
                                                         e.ResetPasswordToken == request.ResetPasswordDto.Token &&
                                                         e.ResetPasswordTokenExpiry > DateTime.UtcNow);

            if (employee == null)
            {
                await _logService.LogWarningAsync("Reset password failed: Invalid token or token expired.", null,
                    "ResetPassword", "Employee", 0);
                return false;
            }

            // Cập nhật mật khẩu mới
            //employee.PasswordHash = Encoding.ASCII.GetBytes(BCrypt.Net.BCrypt.HashPassword(request.ResetPasswordDto.NewPassword));
            employee.ResetPasswordToken = null; // Xóa token sau khi reset thành công
            employee.ResetPasswordTokenExpiry = null; // Xóa thời hạn token

            await _userRepository.UpdateAsync(employee);

            await _logService.LogInformationAsync("Password reset successful.", employee.Id.ToString(), "ResetPassword",
                "Employee", employee.Id);

            return true;
        }
    }
}
