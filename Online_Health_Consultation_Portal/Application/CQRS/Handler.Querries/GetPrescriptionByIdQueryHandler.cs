using MediatR;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;

namespace Online_Health_Consultation_Portal.Application.CQRS.Handler.Querries
{
    public class GetPrescriptionByIdQueryHandler(IPrescriptionRepository prescriptionRepository)
        : IRequestHandler<GetPrescriptionByIdQuery, Prescription>
    {
        public async Task<Prescription> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
        {
            return await prescriptionRepository.GetByIdAsync(request.PrescriptionId);
        }
    }
}