namespace Online_Health_Consultation_Portal.Application.Dtos.Auth.RefreshToken
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
