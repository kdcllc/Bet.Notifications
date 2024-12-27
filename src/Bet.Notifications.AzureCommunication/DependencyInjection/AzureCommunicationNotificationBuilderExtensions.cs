using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.AzureCommunication;
using Bet.Notifications.AzureCommunication.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class AzureCommunicationNotificationBuilderExtensions
{
    public static INotificationBuilder AddAzureCommunicationEmail(
        this INotificationBuilder builder,
        string sectionName = nameof(AzureCommunicatioOptions),
        Action<AzureCommunicatioOptions, IConfiguration>? configure = null)
    {
        builder.Services
                .AddOptions<AzureCommunicatioOptions>(builder.Name)
                .Configure<IOptionsMonitor<AzureCommunicatioOptions>>(
                (options, optionsMonitor) =>
                {
                    var o = optionsMonitor.Get(builder.Name);
                    options.EmailConnectionString = o.EmailConnectionString;
                })
                .PostConfigure(options =>
                {
                    // validation
                    if (string.IsNullOrWhiteSpace(options.EmailConnectionString))
                    {
                        throw new ArgumentNullException(nameof(options.EmailConnectionString));
                    }
                });

        builder.Services.AddChangeTokenOptions<AzureCommunicatioOptions>(sectionName, builder.Name, (o, c) => configure?.Invoke(o, c));


        builder.Services.AddTransient<IEmailClientFactory, EmailClientFactory>();


        builder.Services.AddTransient<IEmailMessageHandler, AzureCommunicationEmailMessageHandler>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<AzureCommunicatioOptions>>();
            var factory = sp.GetRequiredService<IEmailClientFactory>();

            return new AzureCommunicationEmailMessageHandler(builder.Name, client);
        });
        return builder;
    }
}
