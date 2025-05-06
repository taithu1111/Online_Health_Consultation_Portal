using MediatR;

namespace Online_Health_Consultation_Portal.Application.CQRS.Command
{
    // Command as a record
    public record SendMessageCommand(int SenderId, int ReceiverId, string Content) : IRequest<int>;

}