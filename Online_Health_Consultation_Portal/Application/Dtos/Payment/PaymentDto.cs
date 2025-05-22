namespace Online_Health_Consultation_Portal.Application.Dtos.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }
}
