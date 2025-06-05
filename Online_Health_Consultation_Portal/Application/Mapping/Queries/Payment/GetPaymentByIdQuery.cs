using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;

namespace Online_Health_Consultation_Portal.Application.Queries.Payment
{
    public class GetPaymentByIdQuery : IRequest<PaymentDto>
    {
        public int Id { get; set; }
    }
}
