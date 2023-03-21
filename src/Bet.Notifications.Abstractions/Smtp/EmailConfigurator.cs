using Bet.Notifications.Abstractions.TemplateRenderers;

namespace Bet.Notifications.Abstractions.Smtp;

public class EmailConfigurator : IEmailConfigurator
{
    private static readonly char[] EmailsSeparator = new char[] { ',', ';' };

    public EmailConfigurator(string name, Address from)
        : this(name, from, new ReplaceTemplateRenderer(string.Empty), new FileSystemEmailMessageHandler("/"))
    {
    }

    public EmailConfigurator(
        string name,
        Address from,
        ITemplateRenderer renderer,
        IEmailMessageHandler sender)
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
    public EmailMessage Message { get; set; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public ITemplateRenderer Renderer { get; set; }

    /// <inheritdoc/>
    public IEmailMessageHandler Sender { get; }

    /// <summary>
    /// Creates a new Email instance and sets the from property.
    /// </summary>
    /// <param name="email">Email address to send from.</param>
    /// <param name="fromName">Name to send from.</param>
    /// <returns>Instance of the Email class.</returns>
    public static IEmailConfigurator From(string email, string fromName = "")
    {
        return new EmailConfigurator(string.Empty, new Address(email, fromName));
    }

    /// <inheritdoc/>
    public IEmailConfigurator SetFrom(string email, string name = "")
    {
        Message.From = new Address(email, name);
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator To(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.To.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator To(string emails)
    {
        foreach (var email in emails.Split(EmailsSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            Message.To.Add(new Address(email));
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator To(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
            Message.To.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator SetPriority(Priority priority = Priority.Normal)
    {
        Message.Priority = priority;
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Cc(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.Cc.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Cc(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
            Message.Cc.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Bcc(string emails, string names = "")
    {
        var addresses = GetAddresses(emails, names);

        foreach (var address in addresses)
        {
            Message.Bcc.Add(address);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Bcc(IEnumerable<Address> emails)
    {
        foreach (var email in emails)
        {
            Message.Bcc.Add(email);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator ReplyTo(string address)
    {
        Message.ReplyTo.Add(new Address(address));
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator ReplyTo(string address, string name)
    {
        Message.ReplyTo.Add(new Address(address, name));
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Subject(string subject)
    {
        Message.Subject = subject;

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Body(string body, bool isHtml = false)
    {
        Message.Body = body;
        Message.IsHtml = isHtml;

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator PlainTextAltBody(string body)
    {
        Message.PlainTextAltBody = body;
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Attach(Attachment attachment)
    {
        if (!Message.Attachments.Contains(attachment))
        {
            Message.Attachments.Add(attachment);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Attach(IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments.Where(attachment => !Message.Attachments.Contains(attachment)))
        {
            Message.Attachments.Add(attachment);
        }

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator AttachFromFile(string filename, string attachmentName = "")
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
    public IEmailConfigurator Tag(string tag)
    {
        Message.Tags.Add(tag);
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator Header(string header, string body)
    {
        Message.Headers.Add(header, body);
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator UsingTemplateEngine(ITemplateRenderer renderer)
    {
        Renderer = renderer;
        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator UsingTemplate<T>(string template, T model, bool isHtml = true)
    {
        var result = Renderer.ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
        Message.IsHtml = isHtml;
        Message.Body = result;

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator UsingTemplatePlainTextAlt<T>(string template, T model)
    {
        var result = Renderer.ParseAsync(template, model, false).GetAwaiter().GetResult();
        Message.PlainTextAltBody = result;

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true)
    {
        var template = string.Empty;

        using (var reader = new StreamReader(File.OpenRead(filename)))
        {
            template = reader.ReadToEnd();
        }

        var result = Renderer.ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
        Message.IsHtml = isHtml;
        Message.Body = result;

        return this;
    }

    /// <inheritdoc/>
    public IEmailConfigurator UsingTemplateFromFilePlainTextAlt<T>(string filename, T model)
    {
        var template = string.Empty;

        using (var reader = new StreamReader(File.OpenRead(filename)))
        {
            template = reader.ReadToEnd();
        }

        var result = Renderer.ParseAsync(template, model, false).GetAwaiter().GetResult();
        Message.PlainTextAltBody = result;

        return this;
    }

    /// <inheritdoc/>
    public virtual Task<NotificationResult> SendAsync(CancellationToken cancellationToken = default)
    {
        return Sender.SendAsync(Message, cancellationToken);
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
