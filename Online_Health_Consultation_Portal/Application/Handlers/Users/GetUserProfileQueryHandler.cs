using MediatR;
using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Application.Queries.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly UserManager<User> _userManager;

        public GetUserProfileQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.User);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var dto = new UserProfileDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Gender = user.Gender,
                Role = role
            };

            if (role == "Patient" && user.Patient != null)
            {
                dto.DateOfBirth = user.Patient.DateOfBirth;
                dto.Phone = user.Patient.Phone;
                dto.Address = user.Patient.Address;
            }
            else if (role == "Doctor" && user.Doctor != null)
            {
                dto.Bio = user.Doctor.Bio;
                dto.ExperienceYears = user.Doctor.ExperienceYears;
                dto.Languages = user.Doctor.Languages;
                dto.ConsultationFee = user.Doctor.ConsultationFee;
                
                if (user.Doctor.Specialization != null)
                {
                    dto.Specialization = user.Doctor.Specialization.Name;
                }
            }

            return dto;
        }
    }
}