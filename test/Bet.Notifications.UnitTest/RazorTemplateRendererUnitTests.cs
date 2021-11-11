using System.Dynamic;

using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Razor;
using Bet.Notifications.Razor.Options;
using Bet.Notifications.UnitTest.Models;

using Microsoft.Extensions.Options;

using Moq;

namespace Bet.Notifications.UnitTest;

public class RazorTemplateRendererUnitTests
{
    [Fact]
    public void Create_Body_From_Inline_Template_With_A_Layout_Using_ViewBag()
    {
        var optionsMonitor = Mock.Of<IOptionsMonitor<RazorTemplateRendererOptions>>(_ => _.Get(string.Empty)
                                                                                         == new RazorTemplateRendererOptions { RootDirectory = Directory.GetCurrentDirectory() });

        var configurator = EmailConfigurator.From("test@email.com");
        configurator.UsingTemplateEngine(new RazorTemplateRenderer(optionsMonitor));

        var template = @"
                        @{
	                        Layout = ""./Shared/_Layout.cshtml"";
                        }
                        Testing @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";

        dynamic viewBag = new ExpandoObject();
        viewBag.Title = "Shalom!";

        configurator.To("to@email.com")
            .Subject("Using inline template with a layout")
            .UsingTemplate(template, new ViewModelWithViewBag { Name = "John the Immerser", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag });

        var body = configurator.Message.Body;

        Assert.Contains("<h1>Shalom!</h1>", body);
        Assert.Contains("Testing John the Immerser here is a list 123", body);
    }
}
