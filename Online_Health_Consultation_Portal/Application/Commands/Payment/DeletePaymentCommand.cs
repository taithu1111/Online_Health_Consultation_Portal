using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.Payment
{
    public class DeletePaymentCommand : IRequest<bool>
    {
        public int PaymentId { get; set; }
    }
}
