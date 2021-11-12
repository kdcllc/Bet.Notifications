using RazorLight.Razor;

namespace Bet.Notifications.Razor.Repository;

public class RepositoryRazorLightProject : RazorLightProject
{
    private readonly ITemplateRepository _repository;

    public RepositoryRazorLightProject(ITemplateRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
    {
        return Task.FromResult(Enumerable.Empty<RazorLightProjectItem>());
    }

    public async override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
    {
        var template = await _repository.GetSingleAsync(templateKey);

        return new RepositoryRazorProjectItem(template.Name, template.Content);
    }

    public async Task<IEnumerable<string>> GetKnownKeysAsync()
    {
        var ids = (await _repository.ListAsync()).Select(x => x.Name);
        return ids.Select(x => x);
    }
}
