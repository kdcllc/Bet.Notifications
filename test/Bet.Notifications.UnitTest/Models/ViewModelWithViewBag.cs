using System.Dynamic;

using Bet.Notifications.Razor;

namespace Bet.Notifications.UnitTest.Models;

public class ViewModelWithViewBag : IViewBagModel
{
    public ExpandoObject ViewBag { get; set; }

    public string Name { get; set; }

    public string[] Numbers { get; set; }
}
