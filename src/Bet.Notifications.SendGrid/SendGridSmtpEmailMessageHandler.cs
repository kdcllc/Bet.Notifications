using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Options;

namespace Bet.Notifications.SendGrid;

public class SendGridSmtpEmailMessageHandler : IEmailMessageHandler
{
    private readonly SendGridOptions _options;

    public SendGridSmtpEmailMessageHandler(
        string name,
        IOptions<SendGridOptions> options)
    {
        Name = name;
        _options = options.Value;
    }

    public string Name { get; }

    public Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null)
    {
        throw new NotImplementedException();
    }
}
