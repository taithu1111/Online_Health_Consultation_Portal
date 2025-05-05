using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;

namespace Online_Health_Consultation_Portal.Application.Queries.Schedule
{
    public class GetAvailableSlotsQuery : IRequest<List<AvailableSlotDto>>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
