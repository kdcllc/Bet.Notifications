using Azure.Communication.Email;

namespace Bet.Notifications.AzureCommunication;

/// <summary>
/// The Azure Communication Email Client Factory.
/// </summary>
public interface IEmailClientFactory
{
    EmailClient GetClient(string name);
}
