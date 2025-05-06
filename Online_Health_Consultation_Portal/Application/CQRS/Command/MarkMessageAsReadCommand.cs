using MediatR;

namespace Online_Health_Consultation_Portal.Application.CQRS.Command;

public record MarkMessageAsReadCommand(int MessageId, int UserId) : IRequest;
