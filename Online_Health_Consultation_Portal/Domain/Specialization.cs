using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
