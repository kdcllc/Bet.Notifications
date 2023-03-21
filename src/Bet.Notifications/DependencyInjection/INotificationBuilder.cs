namespace Microsoft.Extensions.DependencyInjection;

public interface INotificationBuilder
{
    /// <summary>
    /// Registered Services.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Name of the registered builder.
    /// </summary>
    string Name { get; }
}
