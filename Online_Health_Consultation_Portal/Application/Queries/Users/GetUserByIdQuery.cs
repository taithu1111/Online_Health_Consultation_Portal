using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Queries.Users
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int UserId { get; set; }

    }
}
