namespace Bet.Notifications.SendGrid.Options;

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public bool IsSandBoxMode { get; set; }

    public bool UseClickTracking { get; set; }

    public bool UseOpenTracking { get; set; }
}
