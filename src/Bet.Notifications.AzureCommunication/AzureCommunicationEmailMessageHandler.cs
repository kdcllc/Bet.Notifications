using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using AzureEmail = Azure.Communication.Email;

namespace Bet.Notifications.AzureCommunication;

public class AzureCommunicationEmailMessageHandler : IEmailMessageHandler
{
    private readonly AzureEmail.EmailClient _client;

    public AzureCommunicationEmailMessageHandler(
        string name,
        IEmailClientFactory emailClientFactory)
    {
        Name = name;
        _client = emailClientFactory.GetClient(name);
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null)
    {
        var emailContent = new AzureEmail.EmailContent(email.Subject)
        {
            PlainText = email.PlainTextAltBody,
            Html = email.Body
        };

        var toRecipients = email.To.Select(a => new AzureEmail.EmailAddress(a.Email, a.Name)).ToList();
        var ccRecipients = email.Cc.Select(a => new AzureEmail.EmailAddress(a.Email, a.Name)).ToList();
        var bccRecipients = email.Bcc.Select(a => new AzureEmail.EmailAddress(a.Email, a.Name)).ToList();

        var emailRecipients = new AzureEmail.EmailRecipients(toRecipients, ccRecipients, bccRecipients);

        var emailMessage = new AzureEmail.EmailMessage(
            senderAddress: email.From.Email,
            emailRecipients,
            emailContent);

        if (email.Attachments.Any())
        {
            foreach (var attachment in email.Attachments)
            {
                var content = new BinaryData(await GetAttachmentBase64StringAsync(attachment.Stream));
                var emailAttachment = new AzureEmail.EmailAttachment(attachment.Filename, attachment.ContentType, content);
                emailMessage.Attachments.Add(emailAttachment);
            }
        }

        try
        {
            var emailSendOperation = await _client.SendAsync(Azure.WaitUntil.Completed, emailMessage, cancellation.GetValueOrDefault());
            var sendResponse = new NotificationResult
            {
                MessageId = emailSendOperation.Id
            };

            if (emailSendOperation.Value.Status == AzureEmail.EmailSendStatus.Succeeded)
            {
                return sendResponse;
            }

            var errorsList = new List<string>
            {
                $"{emailSendOperation.Value.Status}"
            };

            return NotificationResult.Failed(errorsList.ToArray());
        }
        catch (Azure.RequestFailedException ex)
        {
            return NotificationResult.Failed(ex?.Message ?? string.Empty, ex?.InnerException?.Message ?? string.Empty);
        }
    }

    private async Task<string> GetAttachmentBase64StringAsync(Stream stream)
    {
        using (var ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
