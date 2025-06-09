using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Queries.Appointment
{
    public class GetAllAppointmentsQuery : IRequest<List<AppointmentDto>>
    {

    }
}
