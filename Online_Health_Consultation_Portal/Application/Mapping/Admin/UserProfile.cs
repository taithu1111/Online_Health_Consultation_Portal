using AutoMapper;
using Online_Health_Consultation_Portal.Application.Dtos.Admin.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Mappings.Admin
{
    public class AdminUserProfile : Profile
    {
        public AdminUserProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Role, 
                    opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name));
        }
    }
}