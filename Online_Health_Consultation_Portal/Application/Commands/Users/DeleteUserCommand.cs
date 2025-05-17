using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.Users
{
    public sealed record DeleteUserCommand : IRequest<Unit>// Single purpose
    {
        public int UserId { get; init; }
    }
}