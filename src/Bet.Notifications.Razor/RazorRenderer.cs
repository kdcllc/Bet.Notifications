using System.Security.Cryptography;
using System.Text;

using Bet.Notifications.Abstractions.TemplateRenderers;

using RazorLight;
using RazorLight.Razor;

namespace Bet.Notifications.Razor;

public class RazorRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorRenderer(string name, string? root = null)
    {
        Name = name;

        _engine = new RazorLightEngineBuilder()
                    .UseFileSystemProject(root ?? Directory.GetCurrentDirectory())
                    .UseMemoryCachingProvider()
                    .Build();
    }

    public RazorRenderer(string name, RazorLightProject project)
    {
        Name = name;

        _engine = new RazorLightEngineBuilder()
                    .UseProject(project)
                    .UseMemoryCachingProvider()
                    .Build();
    }

    public RazorRenderer(string name, Type embeddedResRootType)
    {
        Name = name;

        _engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(embeddedResRootType)
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
