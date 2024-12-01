using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HouseKeeperApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Admin name", _settings.Username));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart("html") { Text = body };

            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtpClient.SendAsync(email);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
