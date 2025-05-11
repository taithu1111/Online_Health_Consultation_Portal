using MediatR;
using Online_Health_Consultation_Portal.Application.Queries.Prescription;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;

namespace Online_Health_Consultation_Portal.Application.Handlers.Prescription
{
    public class GetPrescriptionsByPatientIdQueryHandler : IRequestHandler<GetPrescriptionsByPatientIdQuery, List<Prescription>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetPrescriptionsByPatientIdQueryHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<List<Prescription>> Handle(GetPrescriptionsByPatientIdQuery request, CancellationToken cancellationToken)
        {
            return await _prescriptionRepository.GetByPatientIdAsync(request.PatientId);
        }
    }
}