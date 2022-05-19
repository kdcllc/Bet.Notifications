namespace Bet.Notifications.Abstractions.Smtp;

public interface IEmailMessageHandler
{
    /// <summary>
    /// Name of the sender to be used.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Send Async Notifications.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null);
}
