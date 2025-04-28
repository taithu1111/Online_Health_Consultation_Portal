using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;

namespace Online_Health_Consultation_Portal.Application.Queries.Doctors
{
    public class GetDoctorListQuery : IRequest<List<DoctorDto>>
    {
        public int? SpecializationId { get; set; }
        public int? MinExperienceYears { get; set; }
        public string Language { get; set; }
    }
}