using Microsoft.AspNetCore.Identity;

namespace Online_Health_Consultation_Portal.Domain
{
    public class User : IdentityUser<int>
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } // Admin, Doctor, Patient
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        //2 trường data mới 
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

    }
}
