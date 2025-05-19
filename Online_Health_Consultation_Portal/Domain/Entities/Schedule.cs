namespace Online_Health_Consultation_Portal.Domain
{
    public class Schedule
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        //public DayOfWeek DayOfWeek { get; set; }
        public DateTime Date { get; set; } // ngày lam việc
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public Doctor Doctor { get; set; }

        public string? Location { get; set; }    // địa điểm khám
        public string? Description { get; set; } // mô tả lịch khám
    }
}
