namespace Bet.Notifications.Abstractions.Smtp;

public class FileSystemEmailMessageHandler : IEmailMessageHandler
{
    private readonly string _directory;

    public FileSystemEmailMessageHandler(string name, string directory) : this(directory)
    {
        Name = name;
        _directory = directory;
    }

    public FileSystemEmailMessageHandler(string directory)
    {
        _directory = directory;
        Name = string.Empty;
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null)
    {
        var random = new Random();
        var filename = Path.Combine(_directory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{random.Next(1000)}.eml");

        using (var sw = new StreamWriter(File.OpenWrite(filename)))
        {
            await sw.WriteLineAsync($"From: {email.From.Name} <{email.From.Email}>");
            await sw.WriteLineAsync($"To: {string.Join(",", email.To.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Cc: {string.Join(",", email.Cc.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Bcc: {string.Join(",", email.Bcc.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"ReplyTo: {string.Join(",", email.ReplyTo.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Subject: {email.Subject}");
            foreach (var dataHeader in email.Headers)
            {
                await sw.WriteLineAsync($"{dataHeader.Key}:{dataHeader.Value}");
            }

            await sw.WriteLineAsync();
            await sw.WriteAsync(email.Body);
        }

        return NotificationResult.Success();
    }
}
