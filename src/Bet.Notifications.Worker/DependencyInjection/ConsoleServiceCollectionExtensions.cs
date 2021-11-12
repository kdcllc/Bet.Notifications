using Bet.Notifications.Worker;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleServiceCollectionExtensions
    {
        public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddScoped<IMain, Main>();

            // register notifications with defaults
            services.AddEmailConfigurator(Notifications.Replace)
                .AddReplaceTempleteRenderer()
                .AddFileSystemEmailMessageHandler();

            services.AddEmailConfigurator(Notifications.RazorDirectory)
                .AddRazorTemplateRenderer()
                .AddFileSystemEmailMessageHandler();

            services.AddEmailConfigurator(Notifications.RazorInMemoryDb)
                .AddInMemoryRazorTemplateRenderer()
                .AddFileSystemEmailMessageHandler();

            services.AddEmailConfigurator(Notifications.ReplaceSendGridApi)
                .AddReplaceTempleteRenderer()
                .AddSendGridApiEmailMessageHandler();

            services.AddEmailConfigurator(Notifications.ReplaceSendGridSmtp)
                .AddReplaceTempleteRenderer()
                .AddSendGridSmtpEmailMessageHandler();
        }
    }
}
