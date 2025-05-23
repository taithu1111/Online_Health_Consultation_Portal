using System.Security.Claims;

namespace Online_Health_Consultation_Portal.Application.Interfaces.Service
{
    public interface ITokenService
    {
        string GenerateAccessToken(int userId, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}
