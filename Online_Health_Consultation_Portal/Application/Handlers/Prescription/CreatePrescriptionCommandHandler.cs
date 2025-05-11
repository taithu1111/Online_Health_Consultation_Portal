using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Prescription;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;

namespace Online_Health_Consultation_Portal.Application.Handlers.Prescription
{
    public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Prescription>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public CreatePrescriptionCommandHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<Prescription> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = new Prescription
            {
                AppointmentId = request.AppointmentId,
                MedicationName = request.MedicationName,
                Dosage = request.Dosage,
                Instructions = request.Instructions
            };

            var createdPrescription = await _prescriptionRepository.CreateAsync(prescription);

            // Add medication details if any
            foreach (var detail in request.MedicationDetails)
            {
                var medicationDetail = new MedicationDetail
                {
                    PrescriptionId = createdPrescription.Id,
                    MedicationName = detail.MedicationName,
                    Dosage = detail.Dosage,
                    Instructions = detail.Instructions
                };

                await _prescriptionRepository.AddMedicationDetailAsync(medicationDetail);
            }

            // Reload the prescription with medication details
            return await _prescriptionRepository.GetByIdAsync(createdPrescription.Id);
        }
    }
}