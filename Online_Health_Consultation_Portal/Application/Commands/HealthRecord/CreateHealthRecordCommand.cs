using MediatR;

namespace Online_Health_Consultation_Portal.Application.Commands.HealthRecord
{
    public class CreateHealthRecordCommand : IRequest<int>
    {
        public int PatientId { get; set; }
        public string RecordType { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public CreateHealthRecordCommand(int patientId, string recordType, string fileUrl, DateTime createdAt)
        {
            PatientId = patientId;
            RecordType = recordType;
            FileUrl = fileUrl;
            CreatedAt = createdAt;
        }
    }
}
