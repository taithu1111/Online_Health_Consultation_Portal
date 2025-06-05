using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;

namespace Online_Health_Consultation_Portal.Application.Commands.Payment
{
    public class UpdatePaymentStatusCommand : IRequest<PaymentDto>
    {
        public int PaymentId { get; set; }
        public string Status { get; set; }
    }
}

