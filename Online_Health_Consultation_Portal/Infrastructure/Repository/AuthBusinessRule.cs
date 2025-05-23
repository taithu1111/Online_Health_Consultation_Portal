using Online_Health_Consultation_Portal.Application.Interfaces.Repository;
using Online_Health_Consultation_Portal.Domain.Entities;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public class AuthBusinessRule : IAuthBusinessRule
    {
        public bool IsRefreshTokenExpired(User user)
        {
            return user.RefreshTokenExpiryTime <= DateTime.UtcNow;
        }
    }
    
}
