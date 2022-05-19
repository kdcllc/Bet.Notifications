using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid;
using Bet.Notifications.SendGrid.DependencyInjection;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using SendGrid;

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
        builder.Services
                .AddOptions<SendGridClientOptions>(builder.Name)
                .Configure<IOptionsMonitor<SendGridOptions>>(
                (options, optionsMonitor) =>
                {
                    var o = optionsMonitor.Get(builder.Name);
                    options.ApiKey = o.ApiKey;
                })
                .PostConfigure(options =>
                {
                    // validation
                    if (string.IsNullOrWhiteSpace(options.ApiKey))
                    {
                        throw new ArgumentNullException(nameof(options.ApiKey));
                    }
                });

        builder.Services.AddHttpClient(builder.Name).ConfigureHttpClient(
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<SendGridOptions>>().Get(builder.Name);
                client.Timeout = options.Timeout;
            });

        builder.Services.AddChangeTokenOptions<SendGridOptions>(sectionName, builder.Name, (o, c) => configure?.Invoke(o, c));

        builder.Services.AddTransient<IEmailMessageHandler, SendGridApiEmailMessageHandler>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<SendGridOptions>>();

            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient(builder.Name);
            var options = sp.GetRequiredService<IOptionsMonitor<SendGridClientOptions>>().Get(builder.Name);

            var sendGridClient = new InjectableSendGridClient(client, options);

            return new SendGridApiEmailMessageHandler(builder.Name, sendGridClient, optionsMonitor);
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
        builder.Services.AddChangeTokenOptions<SendGridOptions>(
            sectionName,
            builder.Name,
            (o, c) => configure?.Invoke(o, c));

        builder.Services.AddTransient<IEmailMessageHandler, SendGridSmtpEmailMessageHandler>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<SendGridOptions>>();

            return new SendGridSmtpEmailMessageHandler(builder.Name, optionsMonitor);
        });

        return builder;
    }
}
