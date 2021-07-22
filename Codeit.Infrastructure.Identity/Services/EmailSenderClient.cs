using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Codeit.Infrastructure.Identity.Config;

namespace Codeit.Infrastructure.Identity.Services
{
    public class EmailSenderClient : IEmailSender
    {
        private readonly IdentitySettings _settings;
        private readonly ILogger<EmailSenderClient> _logger;

        public EmailSenderClient(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _settings = IdentitySettings.GetSettings(configuration ?? throw new ArgumentNullException(nameof(configuration)));
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<EmailSenderClient>();
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_settings.EmailSender.SendGridApiKey, subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            string fromEmail = _settings.EmailSender.SendGridFromEmail;
            string fromName = _settings.EmailSender.SendGridFromName;

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            _logger.LogDebug($"Sendign email to {email}: {message}");
            await client.SendEmailAsync(msg);
        }
    }
}