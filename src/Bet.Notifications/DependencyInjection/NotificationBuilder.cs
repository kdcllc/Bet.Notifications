namespace Microsoft.Extensions.DependencyInjection;

public class NotificationBuilder : INotificationBuilder
{
    public NotificationBuilder(IServiceCollection services, string name)
    {
        Services = services;
        Name = name;
    }

    public IServiceCollection Services { get; }

    public string Name { get; }
}
