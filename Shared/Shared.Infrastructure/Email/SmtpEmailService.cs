using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Shared.Core.Abstractions;
using Shared.Core.Settings;

namespace Shared.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public SmtpEmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPassword),
            EnableSsl = _settings.EnableSsl
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        await client.SendMailAsync(message, cancellationToken);
    }
}
