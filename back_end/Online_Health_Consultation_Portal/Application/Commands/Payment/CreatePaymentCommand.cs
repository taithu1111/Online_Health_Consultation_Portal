using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;

namespace Online_Health_Consultation_Portal.Application.Commands.Payment
{
    public class CreatePaymentCommand : IRequest<PaymentDto>
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

    }
}
