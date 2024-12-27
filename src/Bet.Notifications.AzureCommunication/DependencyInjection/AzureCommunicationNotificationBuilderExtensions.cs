using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.AzureCommunication;
using Bet.Notifications.AzureCommunication.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class AzureCommunicationNotificationBuilderExtensions
{
    /// <summary>
    /// Adds the Azure Communication Email message handler.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sectionName"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Thrown when the EmailConnectionString is null or empty.</exception>
    public static INotificationBuilder AddAzureCommunicationEmailMessageHandler(
        this INotificationBuilder builder,
        string sectionName = nameof(AzureCommunicationOptions),
        Action<AzureCommunicationOptions, IConfiguration>? configure = null)
    {
        builder.Services.AddChangeTokenOptions<AzureCommunicationOptions>(sectionName, builder.Name, (o, c) => configure?.Invoke(o, c));

        builder.Services.AddTransient<IEmailClientFactory, EmailClientFactory>();

        builder.Services.AddTransient<IEmailMessageHandler, AzureCommunicationEmailMessageHandler>(sp =>
        {
            var factory = sp.GetRequiredService<IEmailClientFactory>();

            return new AzureCommunicationEmailMessageHandler(builder.Name, factory);
        });

        return builder;
    }
}
