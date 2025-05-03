using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }

        public Appointment Appointment { get; set; }

        public ICollection<MedicationDetail> MedicationDetails { get; set; } = new List<MedicationDetail>();

    }
}
