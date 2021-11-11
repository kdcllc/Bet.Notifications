namespace Bet.Notifications.Abstractions.Smtp;

public class FileSystemSender : ISender
{
    private readonly string _directory;

    public FileSystemSender(string name, string directory) : this(directory)
    {
        Name = name;
        _directory = directory;
    }

    public FileSystemSender(string directory)
    {
        _directory = directory;
        Name = string.Empty;
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(IEmail email, CancellationToken? cancellation = null)
    {
        var random = new Random();
        var filename = Path.Combine(_directory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{random.Next(1000)}");

        using (var sw = new StreamWriter(File.OpenWrite(filename)))
        {
            await sw.WriteLineAsync($"From: {email.Message.From.Name} <{email.Message.From.Email}>");
            await sw.WriteLineAsync($"To: {string.Join(",", email.Message.To.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Cc: {string.Join(",", email.Message.Cc.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Bcc: {string.Join(",", email.Message.Bcc.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"ReplyTo: {string.Join(",", email.Message.ReplyTo.Select(x => $"{x.Name} <{x.Email}>"))}");
            await sw.WriteLineAsync($"Subject: {email.Message.Subject}");
            foreach (var dataHeader in email.Message.Headers)
            {
                await sw.WriteLineAsync($"{dataHeader.Key}:{dataHeader.Value}");
            }

            await sw.WriteLineAsync();
            await sw.WriteAsync(email.Message.Body);
        }

        return NotificationResult.Success;
    }
}
