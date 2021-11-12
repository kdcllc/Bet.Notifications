using System.Dynamic;

using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.Razor.Repository.EntityFrameworkCore;
using Bet.Notifications.Worker.Models;

namespace Bet.Notifications.Worker;

public class Main : IMain
{
    private readonly ILogger<Main> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IEnumerable<IEmailConfigurator> _emailConfigurators;

    public IConfiguration Configuration { get; set; }

    public Main(
        IHostApplicationLifetime applicationLifetime,
        IConfiguration configuration,
        IEnumerable<IEmailConfigurator> emailConfigurators,
        ILogger<Main> logger)
    {
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _emailConfigurators = emailConfigurators;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> RunAsync()
    {
        _logger.LogInformation("Main executed");

        // use this token for stopping the services
        _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

        var cancellationToken = _applicationLifetime.ApplicationStopping;

        var db = _emailConfigurators.First(x => x.Name == Notifications.RazorInMemoryDb);

        dynamic viewBag = new ExpandoObject();
        viewBag.Title = "Shalom!";

        var testModel = new TestViewModel { Name = "Johny", Age = 33, ViewBag = viewBag };
        await db.To("to@email.com")
                          .Subject("This is test for razor db renderer")
                          .UsingTemplate("testTemplate", testModel)
                          .SendAsync(cancellationToken);

        var repl = _emailConfigurators.First(x => x.Name == Notifications.Replace);

        await repl.To("to@email.com")
                          .Subject("This is test for replace template renderer")
                          .UsingTemplate("Shalom ##Name##", new { Name = "John the Immerser" })
                          .SendAsync(cancellationToken);

        var replSendApi = _emailConfigurators.First(x => x.Name == Notifications.ReplaceSendGridApi);

        await replSendApi.To("kingdavidconsulting@gmail.com")
                          .Subject("This is test for replace template renderer send via SendGrid Api")
                          .UsingTemplate("Shalom ##Name##", new { Name = "John the Immerser" })
                          .SendAsync(cancellationToken);

        var replSendSmtp = _emailConfigurators.First(x => x.Name == Notifications.ReplaceSendGridSmtp);

        await replSendApi.To("kingdavidconsulting@gmail.com")
                          .Subject("This is test for replace template renderer send via SendGrid Smtp")
                          .UsingTemplate("Shalom ##Name##", new { Name = "John the Immerser" })

                          .SendAsync(cancellationToken);
        var razorDirectory = _emailConfigurators.First(x => x.Name == Notifications.RazorDirectory);
        var template = @"
                        @{
	                        Layout = ""./Views/Shared/_Layout.cshtml"";
                        }
                        Shalom @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";

        var model = new ViewModelWithViewBag { Name = "John the Immerser", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };

        await razorDirectory
            .To("email@gmail.com")
            .Subject("This is test for Razor Directory with Template")
            .UsingTemplate(template, model)
            .SendAsync(cancellationToken);

        return 0;
    }
}
