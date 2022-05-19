using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Abstractions.TemplateRenderers;

namespace Microsoft.Extensions.DependencyInjection;

public static class CoreNotificationBuilderServiceExtensions
{
    /// <summary>
    /// Adds <see cref="ReplaceTempleteRenderer"/> for templates ##value## pattern.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static INotificationBuilder AddReplaceTempleteRenderer(this INotificationBuilder builder)
    {
        builder.Services.AddTransient<ITemplateRenderer, ReplaceTempleteRenderer>(sp => new ReplaceTempleteRenderer(builder.Name));

        return builder;
    }

    /// <summary>
    /// Adds <see cref="FileSystemEmailMessageHandler"/> primary for testing purposes.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="path">The path to the templates. The default is `Directory.GetCurrentDirectory()`. For path in bin directory use ' AppContext.BaseDirectory'</param>
    /// <returns></returns>
    public static INotificationBuilder AddFileSystemEmailMessageHandler(this INotificationBuilder builder, string path = "")
    {
        builder.Services.AddTransient<IEmailMessageHandler, FileSystemEmailMessageHandler>(sp =>
        {
            return new FileSystemEmailMessageHandler(builder.Name, string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path);
        });

        return builder;
    }
}
