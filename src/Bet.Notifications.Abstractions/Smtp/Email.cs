using Bet.Notifications.Abstractions.TemplateRenderers;

namespace Bet.Notifications.Abstractions.Smtp;

public class Email : IEmail
{
    private static readonly char[] EmailsSeparator = new char[] { ',', ';' };

    public Email(string name, Address from) : this(name, from, new ReplaceRenderer(string.Empty), new FileSystemSender("/"))
    {
    }

    public Email(
        string name,
        Address from,
        ITemplateRenderer renderer,
        ISender sender)
    {
        Message = new EmailMessage
        {
            From = from,
        };

        Name = name;
        Renderer = renderer;
        Sender = sender;
    }

    /// <inheritdoc/>
    public EmailMessage Message { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public ITemplateRenderer Renderer { get; set; }

    /// <inheritdoc/>
    public ISender Sender { get; }

    /// <summary>
    /// Creates a new Email instance and sets the from property.
    /// </summary>
    /// <param name="email">Email address to send from.</param>
    /// <param name="fromName">Name to send from.</param>
    /// <returns>Instance of the Email class.</returns>
    public static IEmail From(string email, string fromName = "")
    {
        return new Email(string.Empty, new Address(email, fromName));
    }

    /// <inheritdoc/>
    public IEmail To(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.To.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail To(string emails)
    {
        foreach (var email in emails.Split(EmailsSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            Message.To.Add(new Address(email));
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail To(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
            Message.To.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail SetPriority(Priority priority = Priority.Normal)
    {
        Message.Priority = priority;
        return this;
    }

    /// <inheritdoc/>
    public IEmail Cc(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.Cc.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail Cc(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
            Message.Cc.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail Bcc(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.Bcc.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail Bcc(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
           Message.Bcc.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail ReplyTo(string address)
    {
        Message.ReplyTo.Add(new Address(address));
        return this;
    }

    /// <inheritdoc/>
    public IEmail ReplyTo(string address, string name)
    {
        Message.ReplyTo.Add(new Address(address, name));
        return this;
    }

    /// <inheritdoc/>
    public IEmail Subject(string subject)
    {
        Message.Subject = subject;

        return this;
    }

    /// <inheritdoc/>
    public IEmail Body(string body, bool isHtml = false)
    {
        Message.Body = body;
        Message.IsHtml = isHtml;

        return this;
    }

    /// <inheritdoc/>
    public IEmail PlainTextAlternativeBody(string body)
    {
        Message.PlainTextAlternativeBody = body;
        return this;
    }

    /// <inheritdoc/>
    public IEmail Attach(Attachment attachment)
    {
        if (!Message.Attachments.Contains(attachment))
        {
            Message.Attachments.Add(attachment);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail Attach(IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments.Where(attachment => !Message.Attachments.Contains(attachment)))
        {
            Message.Attachments.Add(attachment);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmail AttachFromFilename(string filename, string attachmentName = "")
    {
        var stream = File.OpenRead(filename);
        Attach(new Attachment
        {
            Stream = stream,
            Filename = attachmentName ?? filename
        });

        return this;
    }

    /// <inheritdoc/>
    public IEmail Tag(string tag)
    {
        Message.Tags.Add(tag);
        return this;
    }

    /// <inheritdoc/>
    public IEmail Header(string header, string body)
    {
        Message.Headers.Add(header, body);
        return this;
    }

    /// <inheritdoc/>
    public IEmail UsingTemplateEngine(ITemplateRenderer renderer)
    {
        Renderer = renderer;
        return this;
    }

    /// <inheritdoc/>
    public IEmail UsingTemplate<T>(string template, T model, bool isHtml = true)
    {
        var result = Renderer.ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
        Message.IsHtml = isHtml;
        Message.Body = result;

        return this;
    }

    /// <inheritdoc/>
    public IEmail PlaintextAlternativeUsingTemplate<T>(string template, T model)
    {
        var result = Renderer.ParseAsync(template, model, false).GetAwaiter().GetResult();
        Message.PlainTextAlternativeBody = result;

        return this;
    }

    /// <inheritdoc/>
    public virtual Task<NotificationResult> SendAsync(CancellationToken cancellationToken = default)
    {
        return Sender.SendAsync(this, cancellationToken);
    }

    private static IList<Address> GetAddresses(string emails, string names)
    {
        var result = new List<Address>();

        var nameSplit = !string.IsNullOrEmpty(names) ? names.Split(EmailsSeparator, StringSplitOptions.RemoveEmptyEntries) : new string[0];
        var addressSplit = emails.Split(EmailsSeparator, StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < addressSplit.Length; i++)
        {
            var currentName = string.Empty;
            if ((nameSplit.Length - 1) >= i)
            {
                currentName = nameSplit[i];
            }

            result.Add(new Address(addressSplit[i].Trim(), currentName.Trim()));
        }

        return result;
    }
}
