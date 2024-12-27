using Azure.Communication.Email;

using Bet.Notifications.AzureCommunication.Options;

using Microsoft.Extensions.Options;

namespace Bet.Notifications.AzureCommunication;

public class EmailClientFactory : IEmailClientFactory
{
    private readonly IOptionsMonitor<AzureCommunicationOptions> _optionsMonitor;

    public EmailClientFactory(IOptionsMonitor<AzureCommunicationOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public EmailClient GetClient(string name)
    {
        var options = _optionsMonitor.Get(name);

        var client = new EmailClient(options.EmailConnectionString);

        return client;
    }
}
