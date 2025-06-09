using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Online_Health_Consultation_Portal.Application.Commands.Users
{
    public class UpdateUserProfileCommand : IRequest<bool>
    {
        [JsonIgnore]
        public ClaimsPrincipal? User { get; set; }
        public int? TargetUserId { get; set; } // Optional override for admin updates
        public UpdateUserProfileDto Profile { get; set; } = new();
    }
}