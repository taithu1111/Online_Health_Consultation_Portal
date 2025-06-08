using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Doctors;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Helpers;

namespace Online_Health_Consultation_Portal.Application.Handlers.Doctors
{
    public class AddDoctorCommandHandler : IRequestHandler<AddDoctorCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public AddDoctorCommandHandler(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> Handle(AddDoctorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DoctorDto;

            string? imagePath = null;
            if (dto.ProfileImage != null)
            {
                imagePath = await FileHelper.SaveImageAsync(dto.ProfileImage, "profile_images");
            }

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Gender = dto.Gender,
                Role = "Doctor",
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = imagePath
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded) return false;

            var doctor = new Doctor
            {
                UserId = user.Id,
                ExperienceYears = dto.ExperienceYears,
                Languages = dto.Languages,
                Bio = dto.Bio,
                ConsultationFee = dto.ConsultationFee,
                Specializations = await _context.Specializations
                    .Where(s => dto.SpecializationIds.Contains(s.Id))
                    .ToListAsync(cancellationToken)
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}