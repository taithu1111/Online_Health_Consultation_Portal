using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Application.Queries.Payment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Payment
{
    public class GetPaymentsByAppointmentHandler : IRequestHandler<GetPaymentsByAppointmentQuery, IEnumerable<PaymentDto>>
    {
        private readonly AppDbContext _context;
        public GetPaymentsByAppointmentHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentDto>> Handle(GetPaymentsByAppointmentQuery request, CancellationToken cancellationToken)
        {
            var payments = await _context.Payments
                .AsNoTracking()
                .Where(p => p.AppointmentId == request.AppointmentId)
                .ToListAsync(cancellationToken);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                Amount = p.Amount,
                Status = p.Status,
                TransactionId = p.TransactionId
            });
        }
    }
}
