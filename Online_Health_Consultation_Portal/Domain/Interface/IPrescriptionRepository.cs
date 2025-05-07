using System.Collections.Generic;
using System.Threading.Tasks;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Domain.Interface
{
    public interface IPrescriptionRepository
    {
        Task<Prescription> GetByIdAsync(int id);
        Task<List<Prescription>> GetByPatientIdAsync(int patientId);
        Task<Prescription> CreateAsync(Prescription prescription);
        Task<bool> UpdateAsync(Prescription prescription);
        Task<bool> DeleteAsync(int id);
        Task<MedicationDetail> AddMedicationDetailAsync(MedicationDetail medicationDetail);
    }
}