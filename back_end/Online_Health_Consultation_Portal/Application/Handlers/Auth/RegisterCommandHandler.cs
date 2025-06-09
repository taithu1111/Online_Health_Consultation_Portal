using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public RegisterCommandHandler(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;

            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Gender = dto.Gender,
                Role = dto.Role,
                CreatedAt = dto.CreatedAt,
                ImageUrl = null,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return false;

            if (user.Role == "Patient")
            {
                var patient = new Patient
                {
                    UserId = user.Id,
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth,
                    Address = dto.Address,
                    BloodType = dto.BloodType
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}
