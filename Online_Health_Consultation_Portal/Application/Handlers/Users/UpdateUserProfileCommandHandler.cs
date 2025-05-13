using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public UpdateUserProfileCommandHandler(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
                throw new UnauthorizedAccessException("User context is missing.");

            var user = await _userManager.GetUserAsync(request.User);
            if (user == null)
                throw new Exception("User not found");

            var profile = request.Profile;

            // Update common user fields
            if (!string.IsNullOrWhiteSpace(profile.FullName))
                user.FullName = profile.FullName;

            if (!string.IsNullOrWhiteSpace(profile.Gender))
                user.Gender = profile.Gender;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Patient")
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
                if (patient != null)
                {
                    patient.DateOfBirth = profile.DateOfBirth ?? patient.DateOfBirth;
                    patient.Phone = profile.Phone ?? patient.Phone;
                    patient.Address = profile.Address ?? patient.Address;
                }
            }
            else if (role == "Doctor")
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
                if (doctor != null)
                {
                    doctor.Bio = profile.Bio ?? doctor.Bio;
                    doctor.Languages = profile.Languages ?? doctor.Languages;
                }
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
