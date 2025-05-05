using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.Schedule
{
    public class DeleteScheduleCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
