using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Payment;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Payment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly AppDbContext _context;
        public CreatePaymentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Domain.Payment
            {
                AppointmentId = request.AppointmentId,
                Amount = request.Amount,
                Status = "Pending",
                TransactionId = Guid.NewGuid().ToString()
            };
            _context.Payments.Add(payment);
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
