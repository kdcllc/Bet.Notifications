using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class SendGridNotificationBuilderExtensions
{
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
