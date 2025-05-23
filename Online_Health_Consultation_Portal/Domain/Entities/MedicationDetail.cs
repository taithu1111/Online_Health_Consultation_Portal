using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class MedicationDetail
    {
        [Key]
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }

        // Navigation
        public Prescription Prescription { get; set; }
    }
}
