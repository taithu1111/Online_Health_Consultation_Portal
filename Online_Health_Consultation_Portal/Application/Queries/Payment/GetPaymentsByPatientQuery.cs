using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;

namespace Online_Health_Consultation_Portal.Application.Queries.Payment
{
    public class GetPaymentsByPatientQuery : IRequest<IEnumerable<PaymentDto>>
    {
        public int PatientId { get; set; }
    }
}
