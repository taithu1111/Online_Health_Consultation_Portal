using MediatR;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Queries.Message
{
    public record GetMessagesByConversationIdQuery : IRequest<List<Message>>
    {
        public int ConversationId { get; init; }
        public int SenderId { get; init; }
        public int ReceiverId { get; init; }
        public int OtherUserId { get; init; }
    }
}