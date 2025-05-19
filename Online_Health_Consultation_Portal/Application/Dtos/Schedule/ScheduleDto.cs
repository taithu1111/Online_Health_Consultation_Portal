namespace Online_Health_Consultation_Portal.Application.Dtos.Schedule
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        //public DayOfWeek DayOfWeek { get; set; }
        public string Date { get; set; } // ngày lam việc
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
    }
}
