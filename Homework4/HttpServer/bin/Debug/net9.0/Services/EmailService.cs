// HttpServer.Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using static HttpServer.Services.EmailServiceConst;
namespace HttpServer.Services
{
    public static class EmailService
    {  

        public static async Task SendAsync(string to, string subject, string htmlBody)
        {
            var _cfg = LoadConfig();
            using var message = new MailMessage(
                new MailAddress(_cfg.FromAddr, _cfg.FromName),
                new MailAddress(to))
            {
                Subject = subject,
                Body    = htmlBody,
                IsBodyHtml = true
            };

            using var smtp = new SmtpClient(_cfg.SmtpHost, _cfg.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_cfg.SmtpUser, _cfg.SmtpPass)
            };

            await smtp.SendMailAsync(message);
        }
    }
}