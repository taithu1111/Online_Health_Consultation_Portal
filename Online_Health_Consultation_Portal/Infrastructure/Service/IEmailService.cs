namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
