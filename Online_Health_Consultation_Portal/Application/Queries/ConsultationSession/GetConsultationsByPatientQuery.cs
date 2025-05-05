using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;

namespace Online_Health_Consultation_Portal.Application.Queries.ConsultationSession
{
    public class GetConsultationsByPatientQuery : IRequest<List<ConsultationSessionDto>>
    {
        public int PatientId { get; set; }
    }
}
