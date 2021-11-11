using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Abstractions.TemplateRenderers;
using Bet.Notifications.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class NotificationsServiceCollectionExtensions
{
    /// <summary>
    /// Adds Email Notification Configuration with named options.
    /// The default for the named options is ''.
    /// </summary>
    /// <param name="services">The DI services.</param>
    /// <param name="name">The name of the configurator.</param>
    /// <param name="sectionName">The section of the configurations in <see cref="IConfiguration"/>.</param>
    /// <param name="configureOptions">The default email options.</param>
    /// <returns></returns>
    public static INotificationBuilder AddEmailConfigurator(
        this IServiceCollection services,
        string name = "",
        string sectionName = nameof(EmailOptions),
        Action<EmailOptions, IConfiguration>? configureOptions = null)
    {
        var builder = new NotificationBuilder(services, name);

        builder.Services.AddChangeTokenOptions<EmailOptions>(sectionName, name, (opt, c) => configureOptions?.Invoke(opt, c));

        builder.Services.Add(
            ServiceDescriptor.Transient<IEmailConfigurator>(sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<EmailOptions>>().Get(name);

                var render = sp.GetServices<ITemplateRenderer>().FirstOrDefault(x => x.Name == name);
                var sender = sp.GetServices<IEmailMessageHandler>().FirstOrDefault(x => x.Name == name);

                if (render == null)
                {
                    throw new ArgumentNullException(nameof(render), "Add Template Rederer .AddReplaceTempleteRenderer()");
                }

                if (sender == null)
                {
                    throw new ArgumentNullException(nameof(sender), "Add IEmailMessageHandler .AddReplaceTempleteRenderer()");
                }

                var fromAddress = new Address(options.From, options.FromName);

                return new EmailConfigurator(name, fromAddress, render, sender);
            }));

        return builder;
    }

    public static INotificationBuilder AddReplaceTempleteRenderer(this INotificationBuilder builder)
    {
        builder.Services.AddTransient<ITemplateRenderer, ReplaceTempleteRenderer>(sp => new ReplaceTempleteRenderer(builder.Name));

        return builder;
    }

    public static INotificationBuilder AddFileSystemEmailMessageHandler(this INotificationBuilder builder, string path = "")
    {
        builder.Services.AddTransient<IEmailMessageHandler, FileSystemEmailMessageHandler>(sp =>
        {
            return new FileSystemEmailMessageHandler(builder.Name, string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path);
        });

        return builder;
    }
}
