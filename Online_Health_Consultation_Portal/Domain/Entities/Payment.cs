namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // Paid, Unpaid, Cancelled
        public string TransactionId { get; set; }

        public Appointment Appointment { get; set; }
    }
}
