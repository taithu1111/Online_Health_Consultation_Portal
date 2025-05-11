using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Online_Health_Consultation_Portal.API.DTo
{
    /// <summary>
    /// Data transfer object for creating a new prescription
    /// </summary>
    public record CreatePrescriptionDto
    {
        /// <summary>
        /// The appointment ID for which this prescription is being created
        /// </summary>
        [JsonPropertyName("appointmentId")]
        public int AppointmentId { get; init; }

        /// <summary>
        /// General instructions for the prescription
        /// </summary>
        [JsonPropertyName("instructions")]
        public string Instructions { get; init; }

        /// <summary>
        /// List of medications to include in this prescription
        /// </summary>
        [JsonPropertyName("medications")]
        public List<MedicationItemDto> Medications { get; init; } = new List<MedicationItemDto>();
    }
}