using Azure.Communication.Email;

using Bet.Notifications.AzureCommunication.Options;

using Microsoft.Extensions.Options;

namespace Bet.Notifications.AzureCommunication;

public class EmailClientFactory(IOptionsMonitor<AzureCommunicationOptions> optionsMonitor) : IEmailClientFactory
{
    public EmailClient GetClient(string name)
    {
        var options = optionsMonitor.Get(name);

        var client = new EmailClient(options.EmailConnectionString);

        return client;
    }
}
