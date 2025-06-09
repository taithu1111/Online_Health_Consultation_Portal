using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;

namespace Online_Health_Consultation_Portal.Application.Queries.Appointment
{
    public class GetAppointmentDetailQuery : IRequest<AppointmentDto>
    {
        public int AppointmentId { get; set; }
    }
}
