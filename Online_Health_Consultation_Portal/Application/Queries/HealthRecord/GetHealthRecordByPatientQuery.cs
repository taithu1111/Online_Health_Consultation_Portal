using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;

namespace Online_Health_Consultation_Portal.Application.Queries.HealthRecord
{
    public class GetHealthRecordByPatientQuery : IRequest<List<HealthRecordResponseDto>>
    {
        public int PatientId { get; set; }

        public GetHealthRecordByPatientQuery(int patientId)
        {
            PatientId = patientId;
        }
    }
}
