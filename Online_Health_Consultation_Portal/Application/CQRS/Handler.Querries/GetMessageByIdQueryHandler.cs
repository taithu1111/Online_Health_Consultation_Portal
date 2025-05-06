using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;

namespace Online_Health_Consultation_Portal.Application.CQRS.Handler.Querries
{
    public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, Message>
    {
        private readonly AppDbContext _context;

        public GetMessageByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Messages.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new InvalidOperationException();
        }
    }
}