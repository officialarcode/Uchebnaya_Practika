using System.Net.Mail;
using System.Net;

namespace API_UP2.Services
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetEmailAsync(string email, string resetCode);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetCode)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                if (smtpSettings == null || string.IsNullOrEmpty(smtpSettings["Host"]))
                {
                    _logger.LogError("SMTP settings are not configured");
                    return false;
                }

                using var smtpClient = new SmtpClient(smtpSettings["Host"])
                {
                    Port = int.Parse(smtpSettings["Port"]),
                    Credentials = new NetworkCredential(
                        smtpSettings["Username"],
                        smtpSettings["Password"]),
                    EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"]),
                    Subject = "Восстановление пароля",
                    Body = $"Ваш код для восстановления пароля: {resetCode}. Код действителен в течение 15 минут.",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Код восстановления отправлен на {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке email на {email}");
                return false;
            }
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                if (smtpSettings == null || string.IsNullOrEmpty(smtpSettings["Host"]))
                {
                    _logger.LogError("SMTP settings are not configured");
                    return false;
                }

                using var smtpClient = new SmtpClient(smtpSettings["Host"])
                {
                    Port = int.Parse(smtpSettings["Port"]),
                    Credentials = new NetworkCredential(
                        smtpSettings["Username"],
                        smtpSettings["Password"]),
                    EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке email на {to}");
                return false;
            }
        }
    }
}