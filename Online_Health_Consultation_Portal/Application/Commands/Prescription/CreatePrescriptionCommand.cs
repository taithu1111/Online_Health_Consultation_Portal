using MediatR;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Commands.Prescription;

public record CreatePrescriptionCommand(int AppointmentId , string MedicationName , string Dosage , string Instructions, List<MedicationDetail> MedicationDetails) : IRequest<Prescription>
{
    
}