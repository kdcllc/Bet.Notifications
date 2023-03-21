using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Abstractions.TemplateRenderers;

namespace Microsoft.Extensions.DependencyInjection;

public static class CoreNotificationBuilderServiceExtensions
{
    /// <summary>
    /// Adds <see cref="ReplaceTemplateRenderer"/> for templates ##value## pattern.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static INotificationBuilder AddReplaceTemplateRenderer(this INotificationBuilder builder)
    {
        builder.Services.AddTransient<ITemplateRenderer, ReplaceTemplateRenderer>(_ => new ReplaceTemplateRenderer(builder.Name));

        return builder;
    }

    /// <summary>
    /// Adds <see cref="FileSystemEmailMessageHandler"/> primary for testing purposes.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="path">
    /// The path to the templates. The default is `Directory.GetCurrentDirectory()`.
    /// For path in bin directory use ' AppContext.BaseDirectory'.
    /// </param>
    /// <returns></returns>
    public static INotificationBuilder AddFileSystemEmailMessageHandler(this INotificationBuilder builder, string path = "")
    {
        builder.Services.AddTransient<IEmailMessageHandler, FileSystemEmailMessageHandler>(_ =>
        {
            var filePath = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path;
            return new FileSystemEmailMessageHandler(builder.Name, filePath);
        });

        return builder;
    }
}
