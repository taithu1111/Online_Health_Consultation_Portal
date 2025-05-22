using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Application.Queries.Payment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Payment
{
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
    {
        private readonly AppDbContext _context;
        public GetPaymentByIdHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (payment == null) return null;

            return new PaymentDto
            {
                Id = payment.Id,
                AppointmentId = payment.AppointmentId,
                Amount = payment.Amount,
                Status = payment.Status,
                TransactionId = payment.TransactionId
            };
        }
    } 
}
