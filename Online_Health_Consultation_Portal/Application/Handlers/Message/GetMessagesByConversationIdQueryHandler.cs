using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Application.Queries.Message;

namespace Online_Health_Consultation_Portal.Application.Handlers.Message;

public class GetMessagesByConversationIdQueryHandler(AppDbContext context)
    : IRequestHandler<GetMessagesByConversationIdQuery, List<Message>>
{
    public async Task<List<Message>> Handle(GetMessagesByConversationIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await context.Messages
            .Where(m => m.SenderId == request.SenderId && m.ReceiverId == request.ReceiverId ||
                        m.SenderId == request.OtherUserId && m.ReceiverId == request.SenderId)
            .OrderBy(m => m.SentAt)
            .ToListAsync(cancellationToken);

        // Log the query
        var log = new Log
        {
            Message = $"Retrieved messages for conversation between users {request.SenderId} and {request.OtherUserId}",
            Level = "Information",
            Timestamp = DateTime.UtcNow,
            Action = "Query",
            Entity = "Message",
            EntityId = request.ConversationId
        };

        context.Logs.Add(log);
        await context.SaveChangesAsync(cancellationToken);

        return messages;
    }
}