using System.Security.Cryptography;
using System.Text;

using Bet.Notifications.Abstractions.TemplateRenderers;
using Bet.Notifications.Razor.Options;

using Microsoft.Extensions.Options;

using RazorLight;

namespace Bet.Notifications.Razor;

public class RazorTemplateRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateRenderer(IOptionsMonitor<RazorRendererOptions> optionsMonitor) : this(string.Empty, optionsMonitor)
    {
    }

    public RazorTemplateRenderer(string name, IOptionsMonitor<RazorRendererOptions> optionsMonitor)
    {
        Name = name;

        var builder = new RazorLightEngineBuilder();
        var options = optionsMonitor.Get(name);

        if (!string.IsNullOrEmpty(options.RootDirectory))
        {
            builder.UseFileSystemProject(options.RootDirectory);
        }
        else if (options.Project != null)
        {
            builder.UseProject(options.Project);
        }
        else if (options.EmbeddedResourceRootType != null)
        {
            builder.UseEmbeddedResourcesProject(options.EmbeddedResourceRootType);
        }

        _engine = builder
                    .UseMemoryCachingProvider()
                    .Build();
    }

    public string Name { get; }

    public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default)
    {
        dynamic viewBag = (model as IViewBagModel)?.ViewBag;
        return _engine.CompileRenderStringAsync<T>(GetHashString(template), template, model, viewBag);
    }

    private static string GetHashString(string inputString)
    {
        var sb = new StringBuilder();
        var hashbytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        foreach (var b in hashbytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}
