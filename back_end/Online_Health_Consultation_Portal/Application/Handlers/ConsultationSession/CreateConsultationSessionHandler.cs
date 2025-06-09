using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession
{
    public class CreateConsultationSessionHandler  : IRequestHandler<CreateConsultationSessionCommand, int>
    {
        private readonly AppDbContext _context;
        public CreateConsultationSessionHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateConsultationSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new Domain.ConsultationSession // Tạo mới một buổi tư vấn
            {
                AppointmentId = request.AppointmentId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                SessionNotes = request.SessionNotes,
                MeetingUrl = request.MeetingUrl
            };

            _context.ConsultationSessions.Add(session);
            await _context.SaveChangesAsync(cancellationToken);
            return session.Id; // trả về ID của ConsultationSession vừa tạo
        }
    }
}
