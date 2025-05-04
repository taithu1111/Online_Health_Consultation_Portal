using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Dtos.Paginated;

namespace Online_Health_Consultation_Portal.Application.Queries.Doctors
{
    public class GetDoctorListQuery : IRequest<PaginatedResponse<DoctorDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? SpecializationId { get; set; }
        public int? MinExperienceYears { get; set; }
        public string Language { get; set; }
    }
}
