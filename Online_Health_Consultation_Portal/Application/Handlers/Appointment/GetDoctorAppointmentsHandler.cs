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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetDoctorAppointmentsHandler(AppDbContext context, IAutoMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<AppointmentDto>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value; // lấy role từ token
            if (userRole == "Doctor")
            {
                var doctorId = _httpContextAccessor.HttpContext.User.FindFirst("id")?.Value; // lấy id từ token
                // Lấy danh sách lịch hẹn của bác sĩ từ database
                var appointments = await _context.Appointments
                    .Where(a => a.DoctorId == int.Parse(doctorId))
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<AppointmentDto>>(appointments);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have access!");
            }
        }
    }
}