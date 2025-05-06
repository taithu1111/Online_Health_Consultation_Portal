using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;

namespace Online_Health_Consultation_Portal.Application.Queries.ConsultationSession
{
    public class GetConsultationSessionByIdQuery : IRequest<ConsultationSessionDto>
    {
        public int Id { get; set; }
    }
}
