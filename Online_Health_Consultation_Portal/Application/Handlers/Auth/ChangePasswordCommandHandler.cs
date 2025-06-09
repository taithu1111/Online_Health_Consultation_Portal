using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
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
            var dto = request.ChangePasswordDto;

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                Console.WriteLine("New and confirm password do not match.");
                return false;
            }

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            Console.WriteLine(request.UserId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {errors}");
                Console.WriteLine("Password change failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result.Succeeded;
        }
    }
}
