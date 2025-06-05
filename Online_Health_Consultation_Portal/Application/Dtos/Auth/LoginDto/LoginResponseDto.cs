namespace Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
