using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Online_Health_Consultation_Portal.Application.Commands.Users
{
    public class UpdateUserProfileCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public ClaimsPrincipal? User { get; set; }
        public UpdateUserProfileDto Profile { get; set; } = new();
    }
}