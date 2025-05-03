using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Domain.Interface;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription> GetByIdAsync(int id)
        {
            return (await _context.Prescriptions
                .Include(p => p.Appointment)
                .Include(p => p.MedicationDetails)
                .FirstOrDefaultAsync(p => p.Id == id))!;
        }

        public async Task<List<Prescription>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Prescriptions
                .Include(p => p.Appointment)
                .Include(p => p.MedicationDetails)
                .Where(p => p.Appointment.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Prescription> CreateAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<bool> UpdateAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
                return false;

            _context.Prescriptions.Remove(prescription);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MedicationDetail> AddMedicationDetailAsync(MedicationDetail medicationDetail)
        {
            _context.MedicationDetails.Add(medicationDetail);
            await _context.SaveChangesAsync();
            return medicationDetail;
        }
    }
}
