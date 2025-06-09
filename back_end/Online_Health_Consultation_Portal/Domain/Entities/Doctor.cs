using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; } // FK to User
        
        public int UserId { get; set; }
        public int SpecializationId { get; set; }
        public int ExperienceYears { get; set; }
        public string Languages { get; set; }
        public string Bio { get; set; }
        public decimal ConsultationFee { get; set; }
        public double AverageRating { get; set; }

        public User User { get; set; }
        public Specialization Specialization { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
