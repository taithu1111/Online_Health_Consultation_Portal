using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserProfileCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new UnauthorizedAccessException("User context is missing.");
            var user = await _userManager.GetUserAsync(request.User);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Update common user fields
            if (!string.IsNullOrEmpty(request.FullName) && !string.IsNullOrWhiteSpace(request.FullName)) user.FullName = request.FullName;
            if (!string.IsNullOrEmpty(request.Gender) && !string.IsNullOrWhiteSpace(request.Gender)) user.Gender = request.Gender;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Patient" && user.Patient != null)
            {
                user.Patient.DateOfBirth = request.DateOfBirth ?? user.Patient.DateOfBirth;
                user.Patient.Phone = request.Phone ?? user.Patient.Phone;
                user.Patient.Address = request.Address ?? user.Patient.Address;
            }
            else if (role == "Doctor" && user.Doctor != null)
            {
                user.Doctor.Bio = request.Bio ?? user.Doctor.Bio;
                user.Doctor.Languages = request.Languages ?? user.Doctor.Languages;
            }

            await _userManager.UpdateAsync(user);
            return Unit.Value;
        }
    }
}