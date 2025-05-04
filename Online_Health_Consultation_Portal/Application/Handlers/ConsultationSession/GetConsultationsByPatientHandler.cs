using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession
{
    public class GetConsultationsByPatientHandler : IRequestHandler<GetConsultationsByPatientQuery, List<ConsultationSessionDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetConsultationsByPatientHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ConsultationSessionDto>> Handle(GetConsultationsByPatientQuery request, CancellationToken cancellationToken)
        {
            //var session = await _context.ConsultationSessions
            //    .Include(x => x.Appointment)
            //        .ThenInclude(a => a.Patient)
            //    .Include(x => x.Appointment)
            //        .ThenInclude(a => a.Doctor)
            //    .Where(x => x.Appointment.PatientId == request.PatientId)
            //    .ToListAsync(cancellationToken);

            //return _mapper.Map<List<ConsultationSessionDto>>(session);
            var query = await _context.ConsultationSessions
            .Where(cs => cs.Appointment.PatientId == request.PatientId)
            .Select(cs => new ConsultationSessionDto
            {
                Id = cs.Id,
                AppointmentId = cs.AppointmentId,
                PatientName = cs.Appointment.Patient.User.FullName,
                DoctorName = cs.Appointment.Doctor.User.FullName,
                StartTime = cs.StartTime,
                EndTime = cs.EndTime,
                SessionNotes = cs.SessionNotes,
                MeetingUrl = cs.MeetingUrl,
            })
            .ToListAsync(cancellationToken);
            return query;
        }
    }
}
