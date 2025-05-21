namespace Online_Health_Consultation_Portal.Domain
{
    public class HealthRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public string RecordType { get; set; } // PDF, XRay, etc.
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public Patient Patient { get; set; }
    }
}
