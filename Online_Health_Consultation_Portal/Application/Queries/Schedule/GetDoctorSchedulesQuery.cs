using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;

namespace Online_Health_Consultation_Portal.Application.Queries.Schedule
{
    public class GetDoctorSchedulesQuery : IRequest<List<ScheduleDto>>
    {
        public int DoctorId { get; set; }
    }
}
