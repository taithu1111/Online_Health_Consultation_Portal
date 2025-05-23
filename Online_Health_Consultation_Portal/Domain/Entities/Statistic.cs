namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class Statistic
    {
        public int StatisticId { get; set; }
        public DateTime Date { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalPatients { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
