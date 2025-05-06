namespace Online_Health_Consultation_Portal.Application.Dtos.HealthRecord
{
    //tạo và cập nhật hồ sơ sức khỏe
    public class HealthRecordDto
    {
        public int PatientId { get; set; }
        public string RecordType { get; set; } // PDF, XRay, etc.
        public string FileUrl { get; set; } // URL cho file (PDF, image, etc.)
        public DateTime CreatedAt { get; set; }
    }
}
