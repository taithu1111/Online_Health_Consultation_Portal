using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using System.Linq;

namespace Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession
{
    public class GetConsultationSessionByIdHandler : IRequestHandler<GetConsultationSessionByIdQuery, ConsultationSessionDto>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetConsultationSessionByIdHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ConsultationSessionDto> Handle(GetConsultationSessionByIdQuery request, CancellationToken cancellationToken)
        {
            //var session = await _context.ConsultationSessions
            //    .Include(s => s.Appointment) // Bao gồm thông tin cuộc hẹn liên quan
            //        .ThenInclude(a => a.Patient) // Bao gồm thông tin bệnh nhân
            //    .Include(s => s.Appointment)
            //        .ThenInclude(a => a.Doctor) // Bao gồm thông tin bác sĩ
            //    .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken); // Tìm kiếm buổi tư vấn theo ID

            //if (session == null)
            //{
            //    return null;
            //}

            //return _mapper.Map<ConsultationSessionDto>(session); // Chuyển đổi sang DTO

            var sessions = await _context.ConsultationSessions
                .Where(cs => cs.Id == request.Id)
                .Select(cs => new ConsultationSessionDto
                {
                    Id = cs.Id,
                    AppointmentId = cs.AppointmentId,
                    PatientName = cs.Appointment.Patient.User.FullName,
                    DoctorName = cs.Appointment.Doctor.User.FullName,
                    StartTime = cs.StartTime,
                    EndTime = cs.EndTime,
                    SessionNotes = cs.SessionNotes,
                    MeetingUrl = cs.MeetingUrl
                })
                .FirstOrDefaultAsync(cancellationToken);
            return sessions;
        }
    }
}
