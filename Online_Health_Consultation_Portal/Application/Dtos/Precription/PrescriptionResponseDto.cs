using System;
using System.Collections.Generic;
using Online_Health_Consultation_Portal.Application.Dtos.Medication;

namespace Online_Health_Consultation_Portal.Application.Dtos.Precription
{
    public record PrescriptionResponseDto
    {
        public int Id { get; init; }
        public int AppointmentId { get; init; }
        public int PatientId { get; init; }
        public int DoctorId { get; init; }
        public string PatientName { get; init; }
        public string DoctorName { get; init; }
        public string Notes { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? UpdatedDate { get; init; }
        public List<MedicationItemDto> Medications { get; init; } = new List<MedicationItemDto>();
    }
}