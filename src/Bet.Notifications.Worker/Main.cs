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

        // 1. File System Sender with Razor Template InMemory Database
        //await FileSystemSenderRazorInMemoryDbTemplateAsync(cancellationToken);

        // 2. File System Sender with Replace Template
        //await FileSystemSenderReplaceTemplateAsync(cancellationToken);

        // 3. SendGrid Api Sender with Replace Template.
        // allows for tracking, categories etc.
        //await SendGridSenderReplaceTemplateAsync(Notifications.SendGridApiReplaceTemplate, cancellationToken);

        // 4. SendGrid Smtp Sender with Replace Template
        await SendGridSenderReplaceTemplateAsync(Notifications.SendGridSmtpReplaceTemplate, cancellationToken);

        // 5. File System Sender with
        //await FileSystemSenderRazorTempleInDirectoryAsync(cancellationToken);

        return 0;
    }

    private async Task FileSystemSenderRazorTempleInDirectoryAsync(CancellationToken cancellationToken)
    {
        var configurator = _emailConfigurators.First(x => x.Name == Notifications.FileSystemRazorTemplateInDirectory);

        dynamic viewBag = new ExpandoObject();
        viewBag.Title = "Shalom!";

        var template = @"
                        @{
                         Layout = ""./Views/Shared/_Layout.cshtml"";
                        }
                        Shalom @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";

        var model = new ViewModelWithViewBag { Name = "John the Immerser", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };

        var message = configurator
            .To("email@gmail.com")
            .Subject("This is test for Razor Directory with Template")
            .UsingTemplate(template, model);

        var response = await message.SendAsync(cancellationToken);
        _logger.LogInformation("{methodName}-{result}", nameof(FileSystemSenderRazorTempleInDirectoryAsync), response.Errors.FirstOrDefault());
    }

    private async Task SendGridSenderReplaceTemplateAsync(string name, CancellationToken cancellationToken)
    {
        var configurator = _emailConfigurators.First(x => x.Name == name);

        var model = new { Name = "John the Immerser" };

        var message = configurator.To("kingdavidconsulting@gmail.com")
                          .Subject($"This is a test for replace template renderer send via {name}")
                          .AttachFromFile(@"C:\Users\Root\Downloads\FedEx_WebServices_DevelopersGuide_v2019.pdf", "FedEx_WebServices_DevelopersGuide_v2019.pdf")
                          //.AttachFromFile(@"C:\Users\Root\Downloads\Blazor-for-ASP-NET-Web-Forms-Developers.pdf", "Blazor-for-ASP-NET-Web-Forms-Developers.pdf")
                          .UsingTemplate($"Shalom ##Name## {GetHtml()}", model);
        var response = await message.SendAsync(cancellationToken);

        _logger.LogInformation("{methodName}-{result}", nameof(SendGridSenderReplaceTemplateAsync), response.Errors.FirstOrDefault());
    }

    private async Task FileSystemSenderReplaceTemplateAsync(CancellationToken cancellationToken)
    {
        var configurator = _emailConfigurators.First(x => x.Name == Notifications.FileSytemReplaceTemplate);

        var message = configurator.To("to@email.com")
                          .Subject("This is test for replace template renderer")
                          .UsingTemplate("Shalom ##Name##", new { Name = "John the Immerser" });
        var response = await configurator.SendAsync(cancellationToken);

        _logger.LogInformation("{methodName}-{result}", nameof(FileSystemSenderReplaceTemplateAsync), response.Errors.FirstOrDefault());
    }

    private async Task FileSystemSenderRazorInMemoryDbTemplateAsync(CancellationToken cancellationToken)
    {
        var configurator = _emailConfigurators.First(x => x.Name == Notifications.FileSystemRazorTemplateInMemoryDb);

        dynamic viewBag = new ExpandoObject();
        viewBag.Title = "Shalom!";

        var model = new TestViewModel
        {
            Name = "Johny",
            Age = 33,
            ViewBag = viewBag
        };
        var message = configurator.To("to@email.com")
                          .Subject("This is test for razor db renderer")
                          .UsingTemplate("testTemplate", model);

        var response = await message.SendAsync(cancellationToken);

        _logger.LogInformation("{methodName}-{result}", nameof(FileSystemSenderRazorInMemoryDbTemplateAsync), response.Errors.FirstOrDefault());
    }

    private string GetHtml()
    {
        return File.ReadAllText("email.html");
    }
}
