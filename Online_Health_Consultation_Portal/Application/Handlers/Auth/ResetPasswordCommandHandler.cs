using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain.Entities;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Log> _logRepository;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResetPasswordCommandHandler(IRepository<User> userRepository, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var employees = await _userRepository.GetAllAsync();
            var employee = employees.FirstOrDefault(e => e.Email == request.ResetPasswordDto.Email &&
                                                         e.ResetPasswordToken == request.ResetPasswordDto.Token &&
                                                         e.ResetPasswordTokenExpiry > DateTime.UtcNow);

            var pass = new PasswordHasher<User>();

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            int.TryParse(userIdClaim?.Value, out int currentUserId);

            var newPassword = pass.HashPassword(employee!, request.ResetPasswordDto.NewPassword);

            if (employee == null)
            {
                await _logService.LogWarningAsync("Reset password failed: Invalid token or token expired.", null,
                    "ResetPassword", "Employee", 0);

                return false;
            }

            // Cập nhật mật khẩu mới
            employee.ResetPasswordToken = null; // Xóa token sau khi reset thành công
            employee.ResetPasswordTokenExpiry = null; // Xóa thời hạn token
            employee.PasswordHash = newPassword;

            await _userRepository.UpdateAsync(employee);

            //await _logService.LogInformationAsync("Password reset successful.", employee.Id.ToString(), "ResetPassword",
            //  "Employee", employee.Id);

            await _logService.LogInformationAsync("Password reset successful.",
                currentUserId.ToString(), //
                "ResetPassword",
                "Employee",
                employee.Id);

            await _logService.LogWarningAsync("Reset password failed: Invalid token or token expired.",
                currentUserId.ToString(), // ← thêm dòng này
                "ResetPassword",
                "Employee",
                0);

            //var log = new Log
            //{
            //    Message = "User Reset Password successgfully",
            //    Level = "Information",
            //    Timestamp = DateTime.UtcNow,
            //    UserId = employee.Id.ToString(),
            //    Action = "Reset Password",
            //    Entity = "User",
            //    EntityId = employee.Id
            //};

            //await _logRepository.UpdateAsync(log);

            return true;
        }
    }
}
