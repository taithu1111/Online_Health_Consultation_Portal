using System;

namespace Online_Health_Consultation_Portal.Application.Dtos.Precription
{
    public record PrescriptionListItemDto
    {
        public int Id { get; init; }
        public int AppointmentId { get; init; }
        public string DoctorName { get; init; }
        public DateTime CreatedDate { get; init; }
        public int MedicationCount { get; init; }
    }
}