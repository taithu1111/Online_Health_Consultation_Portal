using System.Text.Json.Serialization;

namespace Online_Health_Consultation_Portal.API.DTo
{
    public record MedicationItemDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; init; }

        [JsonPropertyName("medicationId")]
        public int? MedicationId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("dosage")]
        public string Dosage { get; init; }

        [JsonPropertyName("frequency")]
        public string Frequency { get; init; }

        [JsonPropertyName("duration")]
        public string Duration { get; init; }

        [JsonPropertyName("instructions")]
        public string Instructions { get; init; }
    }
}