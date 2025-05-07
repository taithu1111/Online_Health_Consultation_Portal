using MediatR;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.CQRS.Querries
{
    public class GetPrescriptionsByPatientIdQuery : IRequest<List<Prescription>>
    {

        public int PatientId { get; set; }
    }
}