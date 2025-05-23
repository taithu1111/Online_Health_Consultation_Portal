using Microsoft.AspNetCore.Identity;

namespace Online_Health_Consultation_Portal.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Name { get; set; } // "Patient", "Doctor", "Admin", "Nurse"
        public string Permissions { get; set; } // JSON chứa danh sách quyền

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
