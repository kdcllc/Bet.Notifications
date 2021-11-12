using System.Text;

using MimeKit;

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

    public MimeMessage MimeMessage => FromMailMessage(this);

    public override string ToString()
    {
        using var memory = new MemoryStream();
        MimeMessage.WriteTo(memory);

        var buffer = memory.GetBuffer();
        var count = (int)memory.Length;

        return Encoding.ASCII.GetString(buffer, 0, count);
    }

    private MimeMessage FromMailMessage(EmailMessage message)
    {
        var mimeMessage = new MimeMessage()
        {
            Sender = new MailboxAddress(message.From.Name, message.From.Email)
        };

        foreach (var header in message.Headers)
        {
            mimeMessage.Headers.Add(header.Key, header.Value);
        }

        mimeMessage.From.Add(new MailboxAddress(message.From.Name, message.From.Email));

        foreach (var address in message.To)
        {
            mimeMessage.To.Add(new MailboxAddress(address.Name, address.Email));
        }

        foreach (var address in message.Cc)
        {
            mimeMessage.Cc.Add(new MailboxAddress(address.Name, address.Email));
        }

        foreach (var address in message.Bcc)
        {
            mimeMessage.Bcc.Add(new MailboxAddress(address.Name, address.Email));
        }

        foreach (var address in message.ReplyTo)
        {
            mimeMessage.ReplyTo.Add(new MailboxAddress(address.Name, address.Email));
        }

        mimeMessage.Subject = message.Subject;

        var body = new BodyBuilder();

        if (message.IsHtml)
        {
            body.HtmlBody = message.Body;
        }

        if (string.IsNullOrEmpty(message.PlainTextAltBody))
        {
            body.TextBody = message.PlainTextAltBody;
        }

        foreach (var attachment in message.Attachments)
        {
            // Stream must not be null, otherwise it would try to get the filesystem path
            if (attachment.Stream != null)
            {
                body.Attachments.Add(attachment.Filename, attachment.Stream);
            }
        }

        mimeMessage.Body = body.ToMessageBody();

        return mimeMessage;
    }
}
