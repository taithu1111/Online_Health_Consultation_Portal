using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Infrastructure.Helpers;

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
                User? user = null;

                if (request.TargetUserId.HasValue)
                {
                    // Optional: Check if current user is admin before allowing this
                    var currentUser = await _userManager.GetUserAsync(request.User);
                    var roles = await _userManager.GetRolesAsync(currentUser);

                    user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Id == request.TargetUserId.Value, cancellationToken);
                }
                else
                {
                    if (request.User == null)
                        throw new UnauthorizedAccessException("User context is missing.");

                    user = await _userManager.GetUserAsync(request.User);
                }

                if (user == null)
                {
                    _logger.LogWarning("User not found for profile update");
                    throw new Exception("User not found");
                }

                var profile = request.Profile;

                _logger.LogInformation("Updating profile for user {UserId}", user.Id);

                // Update shared properties
                if (!string.IsNullOrWhiteSpace(profile.FullName))
                {
                    user.FullName = profile.FullName;
                }

                if (!string.IsNullOrWhiteSpace(profile.Phone))
                {
                    // var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, profile.Phone);
                    // if (!setPhoneResult.Succeeded)
                    // {
                    //     _logger.LogWarning("Phone number update failed for user {UserId}: {Errors}", 
                    //         user.Id, string.Join(", ", setPhoneResult.Errors.Select(e => e.Description)));
                    //     return false;
                    // }

                    user.PhoneNumber = profile.Phone;
                    user.PhoneNumberConfirmed = true;
                    _logger.LogInformation("Phone after update for user {UserId}: {Phone}", user.Id, user.PhoneNumber);
                }
                if (profile.ProfileImage != null)
                {
                    var imageUrl = await FileHelper.SaveImageAsync(profile.ProfileImage, "uploads/profile_images");
                    user.ImageUrl = imageUrl;
                    _logger.LogInformation("Updated profile image for user {UserId}: {ImageUrl}", user.Id, imageUrl);
                }

                var primaryRole = user.Role;

                if (primaryRole == "Patient")
                {
                    await HandlePatientProfile(user.Id, profile, cancellationToken);
                }
                else if (primaryRole == "Doctor")
                {
                    await HandleDoctorProfile(user.Id, profile, cancellationToken);
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("User update failed for {UserId}: {Errors}",
                        user.Id, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                    return false;
                }

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        await entry.ReloadAsync(cancellationToken);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
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
                    Address = profile.Address,
                    DateOfBirth = profile.DateOfBirth ?? default,
                    BloodType = profile.BloodType,
                    Gender = profile.Gender
                };
                _context.Patients.Add(patient);
                _logger.LogInformation("Created new patient profile for user {UserId}", userId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(profile.Address))
                    patient.Address = profile.Address;

                if (profile.DateOfBirth.HasValue)
                    patient.DateOfBirth = profile.DateOfBirth.Value;

                if (!string.IsNullOrWhiteSpace(profile.BloodType))
                    patient.BloodType = profile.BloodType;
                
                _logger.LogInformation("Updated existing patient profile for user {UserId}", userId);
            }
        }

        private async Task HandleDoctorProfile(int userId, UpdateUserProfileDto profile, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specializations) // Include Specializations to manage them
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            if (doctor == null)
            {
                doctor = new Doctor
                {
                    UserId = userId,
                    Bio = profile.Bio,
                    Languages = profile.Languages
                };

                if (profile.Specializations != null && profile.Specializations.Any())
                {
                    var specializationEntities = await _context.Specializations
                        .Where(s => profile.Specializations.Contains(s.Name))
                        .ToListAsync(cancellationToken);

                    doctor.Specializations = specializationEntities;

                    // Add doctor to each specialization's Doctors list
                    foreach (var spec in specializationEntities)
                    {
                        if (spec.Doctors == null)
                            spec.Doctors = new List<Doctor>();

                        if (!spec.Doctors.Contains(doctor))
                            spec.Doctors.Add(doctor);
                    }
                }

                if (profile.ExperienceYears.HasValue)
                    doctor.ExperienceYears = profile.ExperienceYears.Value;

                if (profile.ConsultationFee.HasValue)
                    doctor.ConsultationFee = profile.ConsultationFee.Value;

                _context.Doctors.Add(doctor);
                _logger.LogInformation("Created new doctor profile for user {UserId}", userId);
            }
            else
            {
                if (profile.Specializations != null && profile.Specializations.Any())
                {
                    // Load old specializations
                    var oldSpecializations = doctor.Specializations.ToList();

                    // Get new specialization entities
                    var newSpecializations = await _context.Specializations
                        .Where(s => profile.Specializations.Contains(s.Name))
                        .ToListAsync(cancellationToken);

                    // Clear old specializations and update their Doctors list
                    foreach (var oldSpec in oldSpecializations)
                    {
                        oldSpec.Doctors?.Remove(doctor);
                    }

                    doctor.Specializations.Clear();

                    // Assign new specializations
                    foreach (var newSpec in newSpecializations)
                    {
                        doctor.Specializations.Add(newSpec);

                        if (newSpec.Doctors == null)
                            newSpec.Doctors = new List<Doctor>();

                        if (!newSpec.Doctors.Contains(doctor))
                            newSpec.Doctors.Add(doctor);
                    }
                }

                if (!string.IsNullOrWhiteSpace(profile.Bio))
                    doctor.Bio = profile.Bio;

                if (!string.IsNullOrWhiteSpace(profile.Languages))
                    doctor.Languages = profile.Languages;

                if (profile.ExperienceYears.HasValue)
                    doctor.ExperienceYears = profile.ExperienceYears.Value;

                if (profile.ConsultationFee.HasValue)
                    doctor.ConsultationFee = profile.ConsultationFee.Value;

                _logger.LogInformation("Updated existing doctor profile for user {UserId}", userId);
            }
        }
    }
}