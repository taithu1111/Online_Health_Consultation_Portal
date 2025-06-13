using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;
        public GetAllAppointmentsHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Appointments
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                    .ToListAsync(cancellationToken);
            return list.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorName = a.Doctor?.User?.FullName ?? "Unknown Doctor",
                PatientName = a.Patient?.User?.FullName ?? "Unknown Patient",
                AppointmentDateTime = a.AppointmentDateTime,
                Gender = a.Patient?.Gender ?? "Unknown",
                Email = a.Patient?.User.Email ?? "No Email",
                Phone = a.Patient?.User.PhoneNumber ?? "No Phone",
                Address = a.Patient?.Address ?? "No Address",
                Type = a.Type,
                Notes = a.Notes,
                Status = a.Status,
                Diagnosis = a.Diagnosis
            }).ToList();
        }
    }
}
