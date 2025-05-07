using MediatR;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.CQRS.Querries
{
    public class GetMessageByIdQuery : IRequest<Message>
    {
        public int Id { get; set; }
    }
}