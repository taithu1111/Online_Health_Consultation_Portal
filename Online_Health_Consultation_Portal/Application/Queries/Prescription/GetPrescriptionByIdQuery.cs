using MediatR;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Queries.Prescription
{
    public class GetPrescriptionByIdQuery : IRequest<Prescription>
    {


        public int PrescriptionId { get; set; }
    }
}