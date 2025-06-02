using AutoMapper;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src =>
                    src.Specializations.Select(s => s.Name).ToList()))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.PhoneNumber));
        }
    }
}