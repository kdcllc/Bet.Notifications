namespace Bet.Notifications.SendGrid.Options;

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public bool IsSandBoxMode { get; set; }

    public bool UseClickTracking { get; set; }

    public bool UseOpenTracking { get; set; }

    /// <summary>
    /// Timeout for Smtp.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Port Number for Smtp.
    /// </summary>
    public int Port { get; set; } = 465;
}
