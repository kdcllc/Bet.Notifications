using Bet.Notifications.Abstractions.TemplateRenderers;

namespace Bet.Notifications.Abstractions.Smtp;

public interface IEmailConfigurator
{
    /// <summary>
    /// An Email Message that was build.
    /// </summary>
    EmailMessage Message { get; }

    /// <summary>
    /// A template renderer engine to be used for transformation.
    /// </summary>
    ITemplateRenderer Renderer { get; set; }

    /// <summary>
    /// A registered sender to be used for emails.
    /// </summary>
    IEmailMessageHandler Sender { get; }

    /// <summary>
    /// Name of the specified Email instance.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Adds a recipient to the email, Splits name and address on ';' or ','.
    /// </summary>
    /// <param name="emails">Email address of the recipient.</param>
    /// <param name="names">Name of the recipient.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator To(string emails, string names = "");

    /// <summary>
    /// Adds a recipient to the email.
    /// </summary>
    /// <param name="emails">Email address of recipient (allows multiple splitting on ';' or ',').</param>
    /// <returns></returns>
    IEmailConfigurator To(string emails);

    /// <summary>
    /// Adds all recipients in list to email.
    /// </summary>
    /// <param name="emails">List of recipients.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator To(IEnumerable<Address> emails);

    /// <summary>
    /// Adds a Carbon Copy to the email, Splits name and address on ';' or ','.
    /// </summary>
    /// <param name="emails">Email address to cc.</param>
    /// <param name="names">Name to cc.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Cc(string emails, string names = "");

    /// <summary>
    /// Adds all Carbon Copy in list to an email.
    /// </summary>
    /// <param name="emails">List of recipients to CC.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Cc(IEnumerable<Address> emails);

    /// <summary>
    /// Adds a blind carbon copy to the email, Splits name and address on ';' or ','.
    /// </summary>
    /// <param name="emails">Email address of bcc.</param>
    /// <param name="names">Name of bcc.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Bcc(string emails, string names = "");

    /// <summary>
    /// Adds all blind carbon copy in list to an email.
    /// </summary>
    /// <param name="emails">List of recipients to BCC.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Bcc(IEnumerable<Address> emails);

    /// <summary>
    /// Sets the ReplyTo address on the email.
    /// </summary>
    /// <param name="address">The ReplyTo Address.</param>
    /// <returns></returns>
    IEmailConfigurator ReplyTo(string address);

    /// <summary>
    /// Sets the ReplyTo address on the email.
    /// </summary>
    /// <param name="address">The ReplyTo Address.</param>
    /// <param name="name">The Display Name of the ReplyTo.</param>
    /// <returns></returns>
    IEmailConfigurator ReplyTo(string address, string name);

    /// <summary>
    /// Sets the subject of the email.
    /// </summary>
    /// <param name="subject">email subject.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Subject(string subject);

    /// <summary>
    /// Adds a Body to the Email.
    /// </summary>
    /// <param name="body">The content of the body.</param>
    /// <param name="isHtml">True if Body is HTML, false for plain text (Optional).</param>
    IEmailConfigurator Body(string body, bool isHtml = false);

    /// <summary>
    /// Adds a Plaintext alternative Body to the Email. Used in conjunction with an HTML email,
    /// this allows for email readers without html capability, and also helps avoid spam filters.
    /// </summary>
    /// <param name="body">The content of the body.</param>
    IEmailConfigurator PlainTextAltBody(string body);

    /// <summary>
    /// Sets message priority.
    /// </summary>
    /// <param name="priority"></param>
    /// <returns></returns>
    IEmailConfigurator SetPriority(Priority priority = Priority.Normal);

    /// <summary>
    /// Adds an Attachment to the Email.
    /// </summary>
    /// <param name="attachment">The Attachment to add.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Attach(Attachment attachment);

    /// <summary>
    /// Adds Multiple Attachments to the Email.
    /// </summary>
    /// <param name="attachments">The List of Attachments to add.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Attach(IEnumerable<Attachment> attachments);

    /// <summary>
    /// Attach file from the local file system.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="attachmentName"></param>
    /// <returns></returns>
    IEmailConfigurator AttachFromFile(string filename, string attachmentName = "");

    /// <summary>
    /// Adds tag to the Email.
    /// </summary>
    /// <param name="tag">Tag name, max 128 characters, ASCII only.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Tag(string tag);

    /// <summary>
    /// Adds header to the Email.
    /// </summary>
    /// <param name="header">Header name, only printable ASCII allowed.</param>
    /// <param name="body">value of the header.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator Header(string header, string body);

    /// <summary>
    /// Set the template rendering engine to use, defaults to <see cref="ReplaceTempleteRenderer"/>.
    /// </summary>
    IEmailConfigurator UsingTemplateEngine(ITemplateRenderer renderer);

    /// <summary>
    /// Adds a template and generates the body of the message.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    /// <param name="template">The template.</param>
    /// <param name="model">Model for the template.</param>
    /// <param name="isHtml">True if Body is HTML, false for plain text (Optional).</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator UsingTemplate<T>(string template, T model, bool isHtml = true);

    /// <summary>
    /// Adds the template file to the email.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename">The path to the file to load.</param>
    /// <param name="model">Model for the template.</param>
    /// <param name="isHtml">True if Body is HTML, false for plain text (Optional).</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true);

    /// <summary>
    /// Adds atemplate to the email.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="template">The a template.</param>
    /// <param name="model">Model for the template.</param>
    /// <returns>Instance of the Email class.</returns>
    IEmailConfigurator UsingTemplatePlainTextAlt<T>(string template, T model);

    /// <summary>
    /// Adds the template file to the email.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename">The path to the file to load.</param>
    /// <param name="model">Model for the template.</param>
    /// <returns>Instance of the E.</returns>
    IEmailConfigurator UsingTemplateFromFilePlainTextAlt<T>(string filename, T model);

    /// <summary>
    /// Sends notifications async.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<NotificationResult> SendAsync(CancellationToken cancellationToken = default);
}
