namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Default implementation of <see cref="INotificationBuilder"/>.
/// </summary>
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
