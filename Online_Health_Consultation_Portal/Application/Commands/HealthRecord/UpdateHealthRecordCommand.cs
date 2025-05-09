using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.HealthRecord
{
    public class UpdateHealthRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string RecordType { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public UpdateHealthRecordCommand(int id, int patientId, string recordType, string fileUrl, DateTime createdAt)
        {
            Id = id;
            PatientId = patientId;
            RecordType = recordType;
            FileUrl = fileUrl;
            CreatedAt = createdAt;
        }
    }
}
