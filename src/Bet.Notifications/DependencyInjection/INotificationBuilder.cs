namespace Microsoft.Extensions.DependencyInjection;

public interface INotificationBuilder
{
    public IServiceCollection Services { get; }

    string Name { get; }
}
