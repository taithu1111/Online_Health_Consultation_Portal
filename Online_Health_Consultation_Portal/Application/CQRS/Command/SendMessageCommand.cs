using MediatR;

namespace Online_Health_Consultation_Portal.Application.CQRS.Command
{
    // Command as a record
    public record SendMessageCommand : IRequest<int>
    {
        public int SenderId { get; init; }
        public int ReceiverId { get; init; }
<<<<<<< HEAD
        public required string Content { get; init; }
=======
        public string Content { get; init; }
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378
    }


}
