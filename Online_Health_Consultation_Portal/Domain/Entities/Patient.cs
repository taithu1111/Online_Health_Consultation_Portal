using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } // FK to User

        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string? BloodType { get; set; }

        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}