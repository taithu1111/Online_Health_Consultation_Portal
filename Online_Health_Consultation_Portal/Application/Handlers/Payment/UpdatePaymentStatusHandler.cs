using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Payment;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Payment
{
    public class UpdatePaymentStatusHandler : IRequestHandler<UpdatePaymentStatusCommand, PaymentDto>
    {
        private readonly AppDbContext _context;
        public UpdatePaymentStatusHandler(AppDbContext context) => _context = context;

        public async Task<PaymentDto> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(request.PaymentId);
            if (payment == null) return null;

            payment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);

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
