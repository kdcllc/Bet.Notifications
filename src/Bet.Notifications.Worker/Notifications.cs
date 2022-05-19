using Bet.Notifications.Abstractions.Smtp;

namespace Bet.Notifications.Worker;

/// <summary>
/// Registration for the <see cref="IEmailConfigurator"/>.
/// </summary>
public static class Notifications
{
    public static string FileSytemReplaceTemplate => nameof(FileSytemReplaceTemplate);

    public static string FileSystemRazorTemplateInDirectory => nameof(FileSystemRazorTemplateInDirectory);

    public static string FileSystemRazorTemplateInMemoryDb => nameof(FileSystemRazorTemplateInMemoryDb);

    public static string SendGridApiReplaceTemplate => nameof(SendGridApiReplaceTemplate);

    public static string SendGridSmtpReplaceTemplate => nameof(SendGridSmtpReplaceTemplate);
}
