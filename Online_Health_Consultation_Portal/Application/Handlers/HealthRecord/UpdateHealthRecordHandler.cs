using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.HealthRecord
{
    public class UpdateHealthRecordHandler : IRequestHandler<UpdateHealthRecordCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;

        public UpdateHealthRecordHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = await _context.HealthRecords.FindAsync(request.Id);
            if (healthRecord == null)
            {
                return false;
            }

            _mapper.Map<Domain.HealthRecord>(request); // Map từ UpdateHealthRecordCommand sang entity HealthRecord
            await _context.SaveChangesAsync(cancellationToken);

            return true; 
        }
    }
}
