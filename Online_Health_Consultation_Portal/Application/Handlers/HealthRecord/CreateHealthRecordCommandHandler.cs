using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.HealthRecord
{
    public class CreateHealthRecordCommandHandler : IRequestHandler<CreateHealthRecordCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateHealthRecordCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = new Domain.HealthRecord
            {
                PatientId = request.PatientId,
                RecordType = request.RecordType,
                FileUrl = request.FileUrl,
                CreatedAt = request.CreatedAt
            };

            _context.HealthRecords.Add(healthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return healthRecord.Id; // trả về ID của HealthRecord vừa tạo
        }
    }

}
