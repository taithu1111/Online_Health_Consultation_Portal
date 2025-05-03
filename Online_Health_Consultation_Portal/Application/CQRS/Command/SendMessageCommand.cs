using MediatR;

namespace Online_Health_Consultation_Portal.Application.CQRS.Command
{
    // Command as a record
    public record SendMessageCommand : IRequest<int>
    {
        public int SenderId { get; init; }
        public int ReceiverId { get; init; }
        public required string Content { get; init; }
    }


}
