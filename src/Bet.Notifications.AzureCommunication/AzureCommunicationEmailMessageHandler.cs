using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using azure = Azure.Communication.Email;

namespace Bet.Notifications.AzureCommunication;

public class AzureCommunicationEmailMessageHandler(
    string name,
    IEmailClientFactory emailClientFactory) : IEmailMessageHandler
{
    public string Name { get; } = name;

    private readonly azure.EmailClient _client = emailClientFactory.GetClient(name);

    public Task<NotificationResult> SendAsync(
        EmailMessage email,
        CancellationToken? cancellation = null)
    {
        throw new NotImplementedException();
    }
}
