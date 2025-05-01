using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.HealthRecord
{
    public class UpdateHealthRecordCommandHandler : IRequestHandler<UpdateHealthRecordCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateHealthRecordCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = await _context.HealthRecords.FindAsync(request.Id);
            if (healthRecord == null)
            {
                return false;
            }

            healthRecord.PatientId = request.PatientId;
            healthRecord.RecordType = request.RecordType;
            healthRecord.FileUrl = request.FileUrl;
            healthRecord.CreatedAt = request.CreatedAt;

            _context.HealthRecords.Update(healthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return true; 
        }
    }
}
