namespace Bet.Notifications.Abstractions.TemplateRenderers;

/// <summary>
/// The template render is used to generate the final text.
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Name of the template.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Parses the template and model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="template"></param>
    /// <param name="model"></param>
    /// <param name="isHtml"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default);
}
