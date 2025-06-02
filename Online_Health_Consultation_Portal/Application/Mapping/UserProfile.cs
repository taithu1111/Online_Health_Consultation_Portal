using AutoMapper;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping cho người dùng xem hồ sơ cá nhân
            CreateMap<UserWithProfile, UserProfileDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.User.ImageUrl ?? null))

                // Patient
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Patient != null ? (DateTime?)src.Patient.DateOfBirth : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Address : null))

                // Doctor
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Bio : null))
                .ForMember(dest => dest.ExperienceYears, opt => opt.MapFrom(src => src.Doctor != null ? (int?)src.Doctor.ExperienceYears : null))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Languages : null))
                .ForMember(dest => dest.ConsultationFee, opt => opt.MapFrom(src => src.Doctor != null ? (decimal?)src.Doctor.ConsultationFee : null))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src =>
                    src.Doctor != null && src.Doctor.Specializations != null
                        ? string.Join(", ", src.Doctor.Specializations.Select(s => s.Name))
                        : null))
                .ForMember(dest => dest.Role, opt => opt.Ignore()); // Gán trong handler

            // Mapping cho admin xem danh sách người dùng
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault() != null
                        ? src.UserRoles.First().Role.Name
                        : string.Empty));
        }
    }
}
