using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;

namespace Online_Health_Consultation_Portal.Application.Queries.Payment
{
    public class GetPaymentsByAppointmentQuery : IRequest<IEnumerable<PaymentDto>>
    {
        public int AppointmentId { get; set; }
    }
}
