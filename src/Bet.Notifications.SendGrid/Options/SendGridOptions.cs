namespace Bet.Notifications.SendGrid.Options;

public class SendGridOptions
{
    /// <summary>
    /// SendGrid Api Key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// SendGrid SandBox Mode.
    /// </summary>
    public bool IsSandBoxMode { get; set; }

    /// <summary>
    /// SendGrid: allow click tracking.
    /// </summary>
    public bool UseClickTracking { get; set; }

    /// <summary>
    /// Sendgrid: allow for open tracking.
    /// </summary>
    public bool UseOpenTracking { get; set; }

    /// <summary>
    /// Timeout for Smtp.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Port Number for Smtp.
    /// </summary>
    public int Port { get; set; } = 465;

    /// <summary>
    /// Allows to throw exceptions.
    /// </summary>
    public bool ThrowException { get; set; }
}
