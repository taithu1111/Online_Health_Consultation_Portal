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

            // Update common user fields
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Gender))
                user.Gender = request.Gender;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Patient")
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
                if (patient != null)
                {
                    patient.DateOfBirth = request.DateOfBirth ?? patient.DateOfBirth;
                    patient.Phone = request.Phone ?? patient.Phone;
                    patient.Address = request.Address ?? patient.Address;
                }
            }
            else if (role == "Doctor")
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
                if (doctor != null)
                {
                    doctor.Bio = request.Bio ?? doctor.Bio;
                    doctor.Languages = request.Languages ?? doctor.Languages;
                }
            }

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
