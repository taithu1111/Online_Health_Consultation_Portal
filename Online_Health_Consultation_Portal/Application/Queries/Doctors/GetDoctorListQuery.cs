using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Dtos.Paginated;

namespace Online_Health_Consultation_Portal.Application.Queries.Doctors
{
    public class GetDoctorListQuery : IRequest<PaginatedResponse<DoctorDto>>
    {
        public DoctorListRequestDto Request { get; set; } = new();
    }
}
