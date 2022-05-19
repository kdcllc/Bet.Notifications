using System.Dynamic;

namespace Bet.Notifications.Razor.Repository.EntityFrameworkCore;

public class TestViewModel : IViewBagModel
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public ExpandoObject? ViewBag { get; set; }
}
