using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class GetDoctorAppointmentsHandler : IRequestHandler<GetDoctorAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetDoctorAppointmentsHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<AppointmentDto>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _context.Appointments
            .Where(a => a.DoctorId == request.DoctorId)  // Chỉ lấy cuộc hẹn của bác sĩ đã cho
            .ToListAsync(cancellationToken);

            return _mapper.Map<List<AppointmentDto>>(appointments);  // Ánh xạ thành List<AppointmentDTO>
        }
    }
}