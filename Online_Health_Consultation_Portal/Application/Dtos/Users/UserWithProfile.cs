using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public class UserWithProfile
    {
        public User User { get; set; }
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}