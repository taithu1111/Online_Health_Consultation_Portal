using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Payment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Payment
{
    public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeletePaymentHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(request.PaymentId);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
