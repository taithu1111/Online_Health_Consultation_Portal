namespace Online_Health_Consultation_Portal.Application.Dtos.Auth.RefreshToken
{
    public class TokenDto
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; }
    }
}
