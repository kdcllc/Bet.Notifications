using Bet.Notifications.Abstractions.TemplateRenderers;
using Bet.Notifications.Razor.Repository;

using RazorLight;

namespace Bet.Notifications.Razor;

public class RazorTemplateDbRenderer : ITemplateRenderer
{
    private RazorLightEngine? _engine;

    public RazorTemplateDbRenderer(string name, RepositoryRazorLightProject project)
    {
        Name = name;

        _engine = new RazorLightEngineBuilder()
            .UseProject(project)
            .Build();
    }

    public string Name { get; }

    public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default)
    {
        if (_engine == null)
        {
            throw new ArgumentNullException(nameof(RazorLightEngine));
        }

        dynamic? viewBag = (model as IViewBagModel)?.ViewBag;
        return _engine.CompileRenderAsync(template, model, viewBag);
    }
}
