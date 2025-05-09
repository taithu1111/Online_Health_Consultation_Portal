using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;

namespace Online_Health_Consultation_Portal.Application.Queries.Appointment
{
    public class GetPatientAppointmentsQuery : IRequest<List<AppointmentDto>>
    {
        public int PatientId { get; set; }
    }
}