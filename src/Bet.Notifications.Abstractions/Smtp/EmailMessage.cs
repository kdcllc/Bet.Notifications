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

    /// <summary>
    /// Create iand instance of <see cref="EmailMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    public static EmailMessage CreateFromMimeMessage(MimeMessage message)
    {
        var output = new EmailMessage
        {
            From = new Address(message.Sender.Address, message.Sender.Name)
        };

        foreach (var header in message.Headers)
        {
            output.Headers.Add(header.Field, header.Value);
        }

        foreach (var to in message.To.Mailboxes)
        {
            output.To.Add(new Address(to.Address, to.Name));
        }

        foreach (var cc in message.Cc.Mailboxes)
        {
            output.Cc.Add(new Address(cc.Address, cc.Name));
        }

        foreach (var bcc in message.Bcc.Mailboxes)
        {
            output.Bcc.Add(new Address(bcc.Address, bcc.Name));
        }

        foreach (var replyTo in message.ReplyTo.Mailboxes)
        {
            output.ReplyTo.Add(new Address(replyTo.Address, replyTo.Name));
        }

        output.Subject = message.Subject;
        output.Body = message.HtmlBody;
        output.PlainTextAltBody = message.TextBody;

        foreach (var attachment in message.Attachments)
        {
            if (attachment is MessagePart part1)
            {
                var fileName = attachment.ContentDisposition?.FileName;
                var rfc822 = part1;

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "attached-message.eml";
                }

                var stream = new MemoryStream();
                rfc822.Message.WriteTo(stream);

                output.Attachments.Add(new Attachment
                {
                    Filename = fileName,
                    Stream = stream
                });
            }
            else
            {
                var part = (MimePart)attachment;
                var fileName = part.FileName;

                var stream = new MemoryStream();
                part.Content.DecodeTo(stream);
            }

            switch (message.Priority)
            {
                case MessagePriority.NonUrgent:
                    output.Priority = Priority.Low;
                    break;
                case MessagePriority.Normal:
                    output.Priority = Priority.Normal;
                    break;
                case MessagePriority.Urgent:
                    output.Priority = Priority.High;
                    break;
            }

            return output;
        }

        switch (message.Priority)
        {
            case MessagePriority.NonUrgent:
                output.Priority = Priority.Low;
                break;
            case MessagePriority.Normal:
                output.Priority = Priority.Normal;
                break;
            case MessagePriority.Urgent:
                output.Priority = Priority.High;
                break;
        }

        return output;
    }

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

        mimeMessage.Headers.Add(HeaderId.Encoding, Encoding.UTF8.EncodingName);

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
            body.HtmlBody = message.Body;
        }

        foreach (var attachment in message.Attachments)
        {
            // Stream must not be null, otherwise it would try to get the filesystem path
            if (attachment.Stream != null)
            {
                body.Attachments.Add(attachment.Filename, attachment.Stream, ContentType.Parse(attachment.ContentType));
            }
        }

        mimeMessage.Body = body.ToMessageBody();

        switch (message.Priority)
        {
            case Priority.Low:
                mimeMessage.Priority = MessagePriority.NonUrgent;
                break;
            case Priority.Normal:
                mimeMessage.Priority = MessagePriority.Normal;
                break;
            case Priority.High:
                mimeMessage.Priority = MessagePriority.Urgent;
                break;
        }

        return mimeMessage;
    }
}
