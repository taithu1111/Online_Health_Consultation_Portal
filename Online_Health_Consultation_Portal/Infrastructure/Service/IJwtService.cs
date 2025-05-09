namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public interface IJwtService
    {
        string GenerateToken(int userId, List<string> roles);
        int? ValidateToken(string token);
    }
}
