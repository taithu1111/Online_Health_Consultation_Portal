using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int Value { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
