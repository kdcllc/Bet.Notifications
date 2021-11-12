namespace Bet.Notifications.Razor.Repository;

public class TemplateItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int Version { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset UpdatedOn { get; set; }
}
