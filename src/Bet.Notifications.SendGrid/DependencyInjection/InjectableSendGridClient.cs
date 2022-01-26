using SendGrid;

namespace Bet.Notifications.SendGrid.DependencyInjection;

/// <summary>
/// A wrapped SendGridClient with single constructor to inject an <see cref="HttpClient"/> whose lifetime is managed externally, e.g. by an DI container.
/// </summary>
internal class InjectableSendGridClient : BaseClient
{
    public InjectableSendGridClient(
        HttpClient httpClient,
        SendGridClientOptions options) : base(httpClient, options)
    {
    }
}
