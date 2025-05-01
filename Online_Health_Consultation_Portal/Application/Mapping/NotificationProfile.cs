using AutoMapper;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Application.Dtos.Notifications;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message ?? "No message available."));
    }
}
