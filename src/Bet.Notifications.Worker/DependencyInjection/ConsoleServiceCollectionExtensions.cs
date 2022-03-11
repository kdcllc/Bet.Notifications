using Bet.Notifications.Worker;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleServiceCollectionExtensions
{
    public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddScoped<IMain, Main>();

        // register notifications with defaults
        services.AddEmailConfigurator(Notifications.FileSytemReplaceTemplate)
            .AddReplaceTempleteRenderer()
            .AddFileSystemEmailMessageHandler();

        services.AddEmailConfigurator(Notifications.FileSystemRazorTemplateInDirectory)
            .AddRazorTemplateRenderer()
            .AddFileSystemEmailMessageHandler();

        services.AddEmailConfigurator(Notifications.FileSystemRazorTemplateInMemoryDb)
            .AddInMemoryRazorTemplateRenderer()
            .AddFileSystemEmailMessageHandler();

        services.AddEmailConfigurator(Notifications.SendGridApiReplaceTemplate)
            .AddReplaceTempleteRenderer()
            .AddSendGridApiEmailMessageHandler(
            configure: (options, config) =>
            {
                options.Timeout = TimeSpan.FromSeconds(200);
            });

        services.AddEmailConfigurator(Notifications.SendGridSmtpReplaceTemplate)
            .AddReplaceTempleteRenderer()
            .AddSendGridSmtpEmailMessageHandler();
    }
}
