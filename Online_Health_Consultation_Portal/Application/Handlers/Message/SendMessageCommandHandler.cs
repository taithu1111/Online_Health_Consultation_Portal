using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Message;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Enum;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Message;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, int>
{
    private readonly AppDbContext _context;
    private readonly ILogger<SendMessageCommandHandler> _logger;

    public SendMessageCommandHandler(AppDbContext context, ILogger<SendMessageCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            SentAt = DateTime.UtcNow,
            IsRead = false,
            Status = MessageStatus.Sent
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        // Log the message sending
        var log = new Log
        {
            Message = $"Message sent from user {request.SenderId} to user {request.ReceiverId}",
            Level = "Information",
            Timestamp = DateTime.UtcNow,
            UserId = request.SenderId.ToString(),
            Action = "Create",
            Entity = "Message",
            EntityId = message.Id
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}