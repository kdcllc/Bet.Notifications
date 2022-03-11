using System.Diagnostics;

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
        using var smtp = new SmtpClient();

        try
        {
            var response = string.Empty;

            smtp.MessageSent += (sender, args) => response = args.Response;

            // custom timeout for large files
            smtp.Timeout = (int)_options.Timeout.TotalSeconds;

            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // 587
            // 465
            await smtp.ConnectAsync("smtp.sendgrid.net", _options.Port, SecureSocketOptions.Auto);
            smtp.AuthenticationMechanisms.Remove("XOAUTH");
            await smtp.AuthenticateAsync("apikey", _options.ApiKey);

            await Task.Delay(300);

            await smtp.SendAsync(email.MimeMessage);

            return NotificationResult.Success(response);
        }
        catch (Exception ex)
        {
            if (_options.ThrowException)
            {
                throw;
            }

            return NotificationResult.Failed(ex?.Message ?? string.Empty, ex?.InnerException?.Message ?? string.Empty);
        }
        finally
        {
            await smtp.DisconnectAsync(true, cancellation.GetValueOrDefault());
        }
    }
}
