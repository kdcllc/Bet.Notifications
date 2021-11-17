using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class SendGridNotificationBuilderExtensions
{
    /// <summary>
    /// Adds SendGrid Api Sender allows for tracking clicks and opens.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="sectionName">The configuration section name for <see cref="SendGridOptions"/>.</param>
    /// <param name="configure">The action to configure <see cref="SendGridOptions"/> with access to <see cref="IConfiguration"/>.</param>
    /// <returns></returns>
    public static INotificationBuilder AddSendGridApiEmailMessageHandler(
        this INotificationBuilder builder,
        string sectionName = nameof(SendGridOptions),
        Action<SendGridOptions, IConfiguration>? configure = null)
    {
        builder.Services.AddChangeTokenOptions<SendGridOptions>(sectionName, builder.Name, (o, c) => configure?.Invoke(o, c));

        builder.Services.AddTransient<IEmailMessageHandler, SendGridApiEmailMessageHandler>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<SendGridOptions>>();

            return new SendGridApiEmailMessageHandler(builder.Name, optionsMonitor);
        });

        return builder;
    }

    /// <summary>
    /// Adds SendGrid Smpt sender to send regular emails.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="sectionName">The configuration section name for <see cref="SendGridOptions"/>.</param>
    /// <param name="configure">The action to configure <see cref="SendGridOptions"/> with access to <see cref="IConfiguration"/>.</param>
    /// <returns></returns>
    public static INotificationBuilder AddSendGridSmtpEmailMessageHandler(
        this INotificationBuilder builder,
        string sectionName = nameof(SendGridOptions),
        Action<SendGridOptions, IConfiguration>? configure = null)
    {
        builder.Services.AddChangeTokenOptions<SendGridOptions>(sectionName, builder.Name, (o, c) => configure?.Invoke(o, c));

        builder.Services.AddTransient<IEmailMessageHandler, SendGridSmtpEmailMessageHandler>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<SendGridOptions>>();

            return new SendGridSmtpEmailMessageHandler(builder.Name, optionsMonitor);
        });

        return builder;
    }
}
