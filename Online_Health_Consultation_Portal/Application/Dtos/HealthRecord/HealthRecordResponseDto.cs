namespace Online_Health_Consultation_Portal.Application.Dtos.HealthRecord
{
    //trả về dữ liệu hồ sơ sức khỏe
    public class HealthRecordResponseDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string RecordType { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
