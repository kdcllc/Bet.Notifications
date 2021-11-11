using Bet.Notifications.Abstractions.Smtp;

namespace Bet.Notifications.Worker;

public class Main : IMain
{
    private readonly ILogger<Main> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IEnumerable<IEmail> _emails;

    public IConfiguration Configuration { get; set; }

    public Main(
        IHostApplicationLifetime applicationLifetime,
        IConfiguration configuration,
        IEnumerable<IEmail> emails,
        ILogger<Main> logger)
    {
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _emails = emails;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> RunAsync()
    {
        _logger.LogInformation("Main executed");

        // use this token for stopping the services
        _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

        var cancellationToken = _applicationLifetime.ApplicationStopping;

        var replaceEmail = _emails.FirstOrDefault(x => x.Name == "replace");

        var template = "Shalom ##Name##";

        var email = replaceEmail?.To("to@email.com")
                          .Subject("This is test")
                          .UsingTemplate(template, new { Name = "John the Immerser" });

        await email?.SendAsync(cancellationToken);

        return 0;
    }
}
