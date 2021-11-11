namespace Bet.Notifications.SendGrid.Options;

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public bool IsSandBoxMode { get; set; }
}
