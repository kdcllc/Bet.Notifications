using RazorLight.Razor;

namespace Bet.Notifications.Razor.Options;

public class RazorRendererOptions
{
    /// <summary>
    /// The root directory to use for the layouts.
    /// The default is 'Directory.GetCurrentDirectory()'.
    /// This allows for Directory based templates rendering.
    /// </summary>
    public string? RootDirectory { get; set; }

    /// <summary>
    /// This allows for <see cref="RazorLightProject"/> to be passed in order to create templating.
    /// The default is null.
    /// </summary>
    public RazorLightProject? Project { get; set; }

    /// <summary>
    /// The embeded resource within assembly to pass.
    /// The default is null.
    /// </summary>
    public Type? EmbeddedResourceRootType { get; set; }
}
