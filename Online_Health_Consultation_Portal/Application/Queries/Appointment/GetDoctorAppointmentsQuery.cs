using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;

namespace Online_Health_Consultation_Portal.Application.Queries.Appointment
{
    public class GetDoctorAppointmentsQuery : IRequest<List<AppointmentDto>>
    {
        public int DoctorId { get; set; }
    }
}
