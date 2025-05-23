using Online_Health_Consultation_Portal.Domain.Entities;

namespace Online_Health_Consultation_Portal.Application.Interfaces.Repository
{
    public interface IAuthBusinessRule
    {
        bool IsRefreshTokenExpired(User user);
    }
}
