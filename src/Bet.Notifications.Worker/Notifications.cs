using Bet.Notifications.Abstractions.Smtp;

namespace Bet.Notifications.Worker;

/// <summary>
/// Registration for the <see cref="IEmailConfigurator"/>.
/// </summary>
public static class Notifications
{
    /// <summary>
    /// File System Razor to save emails and Template In Memory Db for providing with templates.
    /// </summary>
    public static string FileSystemReplaceTemplate => nameof(FileSystemReplaceTemplate);

    /// <summary>
    /// File System Razor to save emails and Template In Directory for providing with templates.
    /// </summary>
    public static string FileSystemRazorTemplateInDirectory => nameof(FileSystemRazorTemplateInDirectory);

    /// <summary>
    /// File System Razor to save emails and Template In Memory Db for providing with templates.
    /// </summary>
    public static string FileSystemRazorTemplateInMemoryDb => nameof(FileSystemRazorTemplateInMemoryDb);

    /// <summary>
    /// SendGrid API to send out an email that is generated from a template inline text.
    /// </summary>
    public static string SendGridApiReplaceTemplate => nameof(SendGridApiReplaceTemplate);

    /// <summary>
    /// SendGrid SMTP to send out an email that is generated from a template inline text.
    /// </summary>
    public static string SendGridSmtpReplaceTemplate => nameof(SendGridSmtpReplaceTemplate);

    /// <summary>
    /// Azure Communication Services to send out an email that is generated from a template that is stored in Template in Directory.
    /// </summary>
    public static string AzureCommunicationTemplateInDirectory => nameof(AzureCommunicationTemplateInDirectory);
}
