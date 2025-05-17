using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;

namespace Online_Health_Consultation_Portal.Application.Commands.Schedule
{
    public class UpdateScheduleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        //public DayOfWeek DayOfWeek { get; set; }
        public DateTime Date { get; set; } // ngày lam việc
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
