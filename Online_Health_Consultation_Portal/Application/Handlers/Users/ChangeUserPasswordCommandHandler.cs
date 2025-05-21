using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null || request.changePasswordDto == null)
            {
                return false;
            }

            var userIdString = request.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userIdString);

            if (user == null)
            {
                return false;  // User not found
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.changePasswordDto.CurrentPassword ?? string.Empty,
                request.changePasswordDto.NewPassword ?? string.Empty);

            return result.Succeeded;
        }
    }
}
