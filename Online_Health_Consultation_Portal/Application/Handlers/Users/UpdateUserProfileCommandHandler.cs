using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Online_Health_Consultation_Portal.Application.Dtos.Users;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(
            UserManager<User> userManager,
            AppDbContext context,
            ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.User == null)
                {
                    _logger.LogWarning("Attempted profile update without user context");
                    throw new UnauthorizedAccessException("User context is missing.");
                }

                var user = await _userManager.GetUserAsync(request.User);
                if (user == null)
                {
                    _logger.LogWarning("User not found for profile update");
                    throw new Exception("User not found");
                }

                var profile = request.Profile;
                _logger.LogInformation("Updating profile for user {UserId}", user.Id);

                // Update base user properties
                if (!string.IsNullOrWhiteSpace(profile.FullName))
                    user.FullName = profile.FullName;

                if (!string.IsNullOrWhiteSpace(profile.Gender))
                    user.Gender = profile.Gender;

                // Update phone number through proper Identity method
                if (!string.IsNullOrWhiteSpace(profile.Phone))
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, profile.Phone);
                    if (!setPhoneResult.Succeeded)
                    {
                        _logger.LogWarning("Phone number update failed for user {UserId}: {Errors}", 
                            user.Id, string.Join(", ", setPhoneResult.Errors.Select(e => e.Description)));
                        return false;
                    }
                }

                // Handle role-specific updates
                var roles = await _userManager.GetRolesAsync(user);
                var primaryRole = roles.FirstOrDefault();

                if (primaryRole == "Patient")
                {
                    await HandlePatientProfile(user.Id, profile, cancellationToken);
                }
                else if (primaryRole == "Doctor")
                {
                    await HandleDoctorProfile(user.Id, profile, cancellationToken);
                }

                // Save user changes
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("User update failed for {UserId}: {Errors}",
                        user.Id, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                    return false;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user");
                throw;
            }
        }

        private async Task HandlePatientProfile(int userId, UpdateUserProfileDto profile, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

            if (patient == null)
            {
                patient = new Patient
                {
                    UserId = userId,
                    Phone = profile.Phone,
                    Address = profile.Address,
                    DateOfBirth = profile.DateOfBirth ?? default // Direct DateTime? assignment
                };
                _context.Patients.Add(patient);
                _logger.LogInformation("Created new patient profile for user {UserId}", userId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(profile.Phone))
                    patient.Phone = profile.Phone;

                if (!string.IsNullOrWhiteSpace(profile.Address))
                    patient.Address = profile.Address;

                patient.DateOfBirth = profile.DateOfBirth ?? DateTime.MinValue;
                _logger.LogInformation("Updated existing patient profile for user {UserId}", userId);
            }
        }

        private async Task HandleDoctorProfile(int userId, UpdateUserProfileDto profile, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            if (doctor == null)
            {
                doctor = new Doctor
                {
                    UserId = userId,
                    Bio = profile.Bio,
                    Languages = profile.Languages
                };
                _context.Doctors.Add(doctor);
                _logger.LogInformation("Created new doctor profile for user {UserId}", userId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(profile.Bio))
                    doctor.Bio = profile.Bio;

                if (!string.IsNullOrWhiteSpace(profile.Languages))
                    doctor.Languages = profile.Languages;
            }
        }
    }
}