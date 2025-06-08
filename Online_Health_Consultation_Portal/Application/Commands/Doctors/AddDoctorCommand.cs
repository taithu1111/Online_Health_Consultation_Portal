using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;

namespace Online_Health_Consultation_Portal.Application.Commands.Doctors
{
    public class AddDoctorCommand : IRequest<bool>
    {
        public AddDoctorDto DoctorDto { get; set; }

        public AddDoctorCommand(AddDoctorDto dto)
        {
            DoctorDto = dto;
        }
    }
}