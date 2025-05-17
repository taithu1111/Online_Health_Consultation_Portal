using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using System.Security.Claims;

namespace Online_Health_Consultation_Portal.Application.Queries.Users
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public ClaimsPrincipal User { get; set; }
    }
}