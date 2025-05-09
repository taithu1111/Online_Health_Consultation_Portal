using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.ConsultationSession
{
    public class CreateConsultationSessionCommand : IRequest<int>
    {
        public int AppointmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SessionNotes { get; set; }
        public string MeetingUrl { get; set; }
    }
}