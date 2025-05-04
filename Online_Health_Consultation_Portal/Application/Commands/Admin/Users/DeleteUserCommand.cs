using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.Admin.Users
{
    public sealed record DeleteUserCommand : IRequest // Single purpose
    {
        public int UserId { get; init; }
    }
}