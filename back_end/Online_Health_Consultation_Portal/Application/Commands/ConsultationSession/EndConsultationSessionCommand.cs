using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.ConsultationSession
{
    public class EndConsultationSessionCommand : IRequest<bool>
    {
        public int Id { get; set; } // ID của buổi tư vấn
    }
}
