using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.HealthRecord
{
    public class DeleteHealthRecordHandler : IRequestHandler<DeleteHealthRecordCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteHealthRecordHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteHealthRecordCommand request, CancellationToken cancellationToken)
        {
            // Tìm hồ sơ sức khỏe cần xóa
            var healthRecord = await _context.HealthRecords.FindAsync(request.Id);
            if (healthRecord == null)
            {
                // Trả về false nếu không tìm thấy hồ sơ
                return false;
            }

            _context.HealthRecords.Remove(healthRecord);

            await _context.SaveChangesAsync(cancellationToken);

            return true; // Trả về true nếu xóa thành công
        }
    }
}
