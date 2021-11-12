using System.Text;

using RazorLight.Razor;

namespace Bet.Notifications.Razor.Repository;

public class RepositoryRazorProjectItem : RazorLightProjectItem
{
    private string _content;

    public RepositoryRazorProjectItem(string key, string content)
    {
        Key = key;
        _content = content;
    }

    public override string Key { get; }

    public override bool Exists => _content != null;

    public override Stream Read()
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(_content));
    }
}
