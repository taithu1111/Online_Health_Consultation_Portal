using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class GetAppointmentDetailHandler : IRequestHandler<GetAppointmentDetailQuery, AppointmentDto>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetAppointmentDetailHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppointmentDto> Handle(GetAppointmentDetailQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            return _mapper.Map<AppointmentDto>(appointment);  // Ánh xạ thành AppointmentDTO
        }
    }
}
