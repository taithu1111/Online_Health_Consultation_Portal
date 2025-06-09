using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession
{
    public class EndConsultationSessionHandler : IRequestHandler<EndConsultationSessionCommand, bool>
    {
        private readonly AppDbContext _context;

        public EndConsultationSessionHandler(AppDbContext context) {
            _context = context;
        }
        public async Task<bool> Handle(EndConsultationSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.ConsultationSessions.FindAsync(request.Id);
            if (session == null)
            {
                return false; // Không tìm thấy buổi tư vấn
            }
            session.EndTime = DateTime.UtcNow; // Cập nhật thời gian kết thúc
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
