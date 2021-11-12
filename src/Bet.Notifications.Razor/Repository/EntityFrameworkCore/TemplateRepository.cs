using Microsoft.EntityFrameworkCore;

namespace Bet.Notifications.Razor.Repository.EntityFrameworkCore;

public class TemplateRepository : ITemplateRepository
{
    private readonly TemplateDbContext _context;

    public TemplateRepository(TemplateDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<TemplateItem> GetSingleAsync(string templateName, CancellationToken cancellation = default)
    {
        return _context.Items.FirstOrDefaultAsync(x => x.Name == templateName, cancellation);
    }

    public async Task<IEnumerable<TemplateItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Items.ToListAsync(cancellationToken);
    }
}
