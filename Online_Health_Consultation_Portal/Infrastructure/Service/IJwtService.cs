namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public interface IJwtService
    {
        string GenerateToken(int userId, List<string> roles);
        int? ValidateToken(string token);

        Task<string> GenerateRefreshTokenAsync();
        Task<bool> SaveRefreshTokenAsync(int userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(int userId, string refreshToken);
    }
}
