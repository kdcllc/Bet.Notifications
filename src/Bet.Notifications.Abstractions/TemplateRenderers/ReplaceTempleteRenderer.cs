using System.Reflection;

namespace Bet.Notifications.Abstractions.TemplateRenderers;

public class ReplaceTemplateRenderer : ITemplateRenderer
{
    public ReplaceTemplateRenderer(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Parse(template, model, isHtml));
    }

    private string Parse<T>(string template, T model, bool isHtml = true)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        foreach (var pi in model.GetType().GetRuntimeProperties())
        {
            template = template.Replace($"##{pi.Name}##", pi.GetValue(model, null).ToString());
        }

        return template;
    }
}
