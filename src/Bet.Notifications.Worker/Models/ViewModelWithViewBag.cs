using System.Dynamic;

using Bet.Notifications.Razor;

namespace Bet.Notifications.Worker.Models;

public class ViewModelWithViewBag : IViewBagModel
{
    public ExpandoObject ViewBag { get; set; } = new ();

    public string Name { get; set; } = string.Empty;

    public string[] Numbers { get; set; } = [];
}
