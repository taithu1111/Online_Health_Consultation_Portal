using AutoMapper;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserWithProfile, UserProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
                
                // Patient
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Patient != null ? (DateTime?)src.Patient.DateOfBirth : null))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Phone : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Address : null))

                // Doctor
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Bio : null))
                .ForMember(dest => dest.ExperienceYears,
                    opt => opt.MapFrom(src => src.Doctor != null ? (int?)src.Doctor.ExperienceYears : null))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Languages : null))
                .ForMember(dest => dest.ConsultationFee,
                    opt => opt.MapFrom(src => src.Doctor != null ? (decimal?)src.Doctor.ConsultationFee : null))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Specialization.Name : null))

                .ForMember(dest => dest.Role, opt => opt.Ignore()); // vẫn để Ignore để handler gán
        }
    }
}
