namespace Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto
{
    public class RegisterDto
    {
        public string FullName { get; set; } // hoặc FirstName + LastName nếu bạn tách
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
