using System.ComponentModel.DataAnnotations;

namespace Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } // hoặc FirstName + LastName nếu bạn tách
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Role { get; set; }
        public string? BloodType { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
