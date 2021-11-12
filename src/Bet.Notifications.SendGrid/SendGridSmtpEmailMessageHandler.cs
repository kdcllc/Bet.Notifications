using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid.Options;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

namespace Bet.Notifications.SendGrid;

public class SendGridSmtpEmailMessageHandler : IEmailMessageHandler
{
    private readonly SendGridOptions _options;

    public SendGridSmtpEmailMessageHandler(
        string name,
        IOptionsMonitor<SendGridOptions> optionsMonitor)
    {
        Name = name;
        _options = optionsMonitor.Get(Name);
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync("smtp.sendgrid.net", 465, SecureSocketOptions.Auto);
            smtp.AuthenticationMechanisms.Remove("XOAUTH");
            await smtp.AuthenticateAsync("apikey", _options.ApiKey);

            await smtp.SendAsync(email.MimeMessage).ConfigureAwait(false);

            await smtp.DisconnectAsync(true);

            return NotificationResult.Success;
        }
        catch (Exception ex)
        {
            return NotificationResult.Failed(ex.Message);
        }
    }
}
