namespace Bet.Notifications.Razor.Repository;

public interface ITemplateRepository
{

    /// <summary>
    /// Get a single template from the database.
    /// </summary>
    /// <param name="templateName"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<TemplateItem> GetSingleAsync(string templateName, CancellationToken cancellation = default);

    /// <summary>
    /// List all of the templates from the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<TemplateItem>> ListAsync(CancellationToken cancellationToken = default);
}
