namespace Bet.Notifications.Razor.Repository;

public interface ITemplateRepository
{
    Task<TemplateItem> GetSingleAsync(string templateName, CancellationToken cancellation = default);

    Task<IEnumerable<TemplateItem>> ListAsync(CancellationToken cancellationToken = default);
}
