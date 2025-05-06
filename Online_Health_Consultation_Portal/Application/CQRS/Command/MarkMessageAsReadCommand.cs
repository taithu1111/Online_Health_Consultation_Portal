using MediatR;

namespace Online_Health_Consultation_Portal.Application.CQRS.Command;

public record MarkMessageAsReadCommand : IRequest
{
    public int MessageId { get; init; }
    public int UserId { get; init; }
}