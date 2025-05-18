using System.Net.Mail;
using System.Net;

namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _smtpSenderEmail;

        public EmailService(IConfiguration configuration)
        {
            //_smtpServer = configuration["EmailSettings:SmtpServer"];
            //_smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            //_smtpUsername = configuration["EmailSettings:SmtpUsername"];s
            //_smtpPassword = configuration["EmailSettings:SmtpPassword"];

            _smtpServer = configuration["EmailSettings:Host"]!;
            _smtpPort = int.Parse(configuration["EmailSettings:Port"]!);
            _smtpUsername = configuration["EmailSettings:Username"]!;
            _smtpPassword = configuration["EmailSettings:Password"]!;
            _smtpSenderEmail = configuration["EmailSettings:SenderEmail"]!;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            Console.WriteLine($"Attempting to send email to: {email}");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Recipient email cannot be null or empty", nameof(email));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Email subject cannot be null or empty", nameof(subject));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Email message cannot be null or empty", nameof(message));
            if (string.IsNullOrWhiteSpace(_smtpSenderEmail))
                throw new InvalidOperationException("SMTP sender email is not configured");

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword), // ✅ FIXED
                EnableSsl = true, // Required for STARTTLS
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSenderEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("[EmailService] Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"[EmailService] SMTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] General Error: {ex.Message}");
                throw;
            }
        }
    }
}
