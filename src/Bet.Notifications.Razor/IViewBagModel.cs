using System.Dynamic;

namespace Bet.Notifications.Razor;

public interface IViewBagModel
{
    ExpandoObject? ViewBag { get; }
}
