using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;
using Online_Health_Consultation_Portal.Application.Queries.HealthRecord;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.HealthRecord
{
    public class GetHealthRecordByPatientQueryHandler : IRequestHandler<GetHealthRecordByPatientQuery, List<HealthRecordResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetHealthRecordByPatientQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthRecordResponseDto>> Handle(GetHealthRecordByPatientQuery request, CancellationToken cancellationToken)
        {
            var healthRecords = await _context.HealthRecords
                .Where(r => r.PatientId == request.PatientId)
                .Select(r => new HealthRecordResponseDto
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    RecordType = r.RecordType,
                    FileUrl = r.FileUrl,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return healthRecords;
        }
    }
}
