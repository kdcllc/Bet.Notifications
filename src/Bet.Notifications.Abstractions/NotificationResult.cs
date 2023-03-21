namespace Bet.Notifications.Abstractions;

/// <summary>
/// Represents the result of sending a notification.
/// </summary>
public class NotificationResult
{
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// An <see cref="IEnumerable{T}"/> containing an errors that occurred during the operation.
    /// </summary>
    public IEnumerable<string> Errors { get; protected set; } = new List<string>();

    /// <summary>
    /// Whether if the operation succeeded or not.
    /// </summary>
    public bool Succeeded => !Errors.Any();

    /// <summary>
    /// Creates an <see cref="NotificationResult"/> indicating a failed Smtp operation, with a list of errors if applicable.
    /// </summary>
    /// <param name="errors">An optional array of <see cref="string"/> which caused the operation to fail.</param>
    public static NotificationResult Failed(params string[] errors)
    {
        return new NotificationResult { Errors = errors };
    }

    /// <summary>
    /// Returns an <see cref="NotificationResult"/>indicating a successful operation.
    /// </summary>
    public static NotificationResult Success(string messageId = "")
    {
        if (string.IsNullOrEmpty(messageId))
        {
            return new NotificationResult();
        }

        return new NotificationResult { MessageId = messageId };
    }
}
