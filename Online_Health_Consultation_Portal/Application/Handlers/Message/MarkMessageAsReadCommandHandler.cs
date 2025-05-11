using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Message;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Message;

public class MarkMessageAsReadCommandHandler : IRequestHandler<MarkMessageAsReadCommand>
{
    private readonly AppDbContext _context;

    public MarkMessageAsReadCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.Id == request.MessageId && m.ReceiverId == request.UserId, cancellationToken);

        if (message != null && !message.IsRead)
        {
            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
            message.Status = Domain.Enum.MessageStatus.Read;

            await _context.SaveChangesAsync(cancellationToken);

            // Log the action
            var log = new Log
            {
                Message = $"Message {request.MessageId} marked as read by user {request.UserId}",
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                UserId = request.UserId.ToString(),
                Action = "Update",
                Entity = "Message",
                EntityId = request.MessageId
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}