namespace Bet.Notifications.Abstractions.Smtp;

public sealed class EmailMessage
{
    public IList<Address> To { get; set; } = new List<Address>();

    public IList<Address> Cc { get; set; } = new List<Address>();

    public IList<Address> Bcc { get; set; } = new List<Address>();

    public IList<Address> ReplyTo { get; set; } = new List<Address>();

    public IList<Attachment> Attachments { get; set; } = new List<Attachment>();

    public Address? From { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? PlainTextAltBody { get; set; }

    public Priority Priority { get; set; }

    public IList<string> Tags { get; set; } = new List<string>();

    public bool IsHtml { get; set; }

    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
