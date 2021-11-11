using Bet.Notifications.Worker;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleServiceCollectionExtensions
    {
        public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddScoped<IMain, Main>();

            // register notifications with defaults
            services.AddEmailNotifications("replace")
                .AddDefaultRenderer()
                .AddDefaultSender();
        }
    }
}
