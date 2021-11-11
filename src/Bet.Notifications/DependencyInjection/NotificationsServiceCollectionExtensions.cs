using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Abstractions.TemplateRenderers;
using Bet.Notifications.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class NotificationsServiceCollectionExtensions
{
    public static INotificationBuilder AddEmailNotifications(
        this IServiceCollection services,
        string name = "",
        string sectionName = nameof(EmailOptions),
        Action<EmailOptions, IConfiguration>? configureOptions = null)
    {
        var builder = new NotificationBuilder(services, name);

        builder.Services.AddChangeTokenOptions<EmailOptions>(sectionName, name, (opt, c) => configureOptions?.Invoke(opt, c));

        builder.Services.Add(
            ServiceDescriptor.Transient<IEmail>(sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<EmailOptions>>().Get(name);
                var render = sp.GetServices<ITemplateRenderer>().FirstOrDefault(x => x.Name == name);
                var sender = sp.GetServices<ISender>().FirstOrDefault(x => x.Name == name);

                var fromAddress = new Address(options.From, options.FromName);

                return new Email(name, fromAddress, render, sender);
            }));

        return builder;
    }

    public static INotificationBuilder AddDefaultRenderer(this INotificationBuilder builder)
    {
        builder.Services.AddTransient<ITemplateRenderer, ReplaceRenderer>(sp => new ReplaceRenderer(builder.Name));

        return builder;
    }

    public static INotificationBuilder AddDefaultSender(this INotificationBuilder builder, string path = "")
    {
        builder.Services.AddTransient<ISender, FileSystemSender>(sp =>
        {
            return new FileSystemSender(builder.Name, string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path);
        });

        return builder;
    }
}
