using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace WorkLink.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = _config["EmailSettings:Host"],
                Port = int.Parse(_config["EmailSettings:Port"]),
                EnableSsl = bool.Parse(_config["EmailSettings:EnableSSL"]),
                UseDefaultCredentials = false, // ✅ IMPORTANT LINE ADDED
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]
                )
            };

            var mail = new MailMessage
            {
                From = new MailAddress(
                    _config["EmailSettings:Username"],
                    "WorkLink"
                ),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EMAIL ERROR: " + ex.Message);
                throw; // rethrow so you can see error in browser
            }
        }
    }
}
