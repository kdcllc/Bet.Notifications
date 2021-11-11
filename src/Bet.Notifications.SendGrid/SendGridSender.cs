using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

using SendGridAttachment = SendGrid.Helpers.Mail.Attachment;

namespace Bet.Notifications.SendGrid;

public class SendGridSender : ISender
{
    private readonly SendGridOptions _options;

    public SendGridSender(string name, IOptions<SendGridOptions> options)
    {
        Name = name;
        _options = options.Value;
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(IEmail email, CancellationToken? cancellation = null)
    {
        var sendGridClient = new SendGridClient(_options.ApiKey);

        var mailMessage = new SendGridMessage();
        mailMessage.SetSandBoxMode(_options.IsSandBoxMode);

        mailMessage.SetFrom(ConvertAddress(email.Message.From));

        if (email.Message.To.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddTos(email.Message.To.Select(ConvertAddress).ToList());
        }

        if (email.Message.Cc.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddCcs(email.Message.Cc.Select(ConvertAddress).ToList());
        }

        if (email.Message.Bcc.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddBccs(email.Message.Bcc.Select(ConvertAddress).ToList());
        }

        if (email.Message.ReplyTo.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            // SendGrid does not support multiple ReplyTo addresses
            mailMessage.SetReplyTo(email.Message.ReplyTo.Select(ConvertAddress).First());
        }

        mailMessage.SetSubject(email.Message.Subject);

        if (email.Message.Headers.Any())
        {
            mailMessage.AddHeaders(email.Message.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        if (email.Message.IsHtml)
        {
            mailMessage.HtmlContent = email.Message.Body;
        }
        else
        {
            mailMessage.PlainTextContent = email.Message.Body;
        }

        switch (email.Message.Priority)
        {
            case Priority.High:

                // https://stackoverflow.com/questions/23230250/set-email-priority-with-sendgrid-api
                mailMessage.AddHeader("Priority", "Urgent");
                mailMessage.AddHeader("Importance", "High");

                // https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmail/2bb19f1b-b35e-4966-b1cb-1afd044e83ab
                mailMessage.AddHeader("X-Priority", "1");
                mailMessage.AddHeader("X-MSMail-Priority", "High");
                break;

            case Priority.Normal:
                // Do not set anything.
                // Leave default values. It means Normal Priority.
                break;

            case Priority.Low:

                // https://stackoverflow.com/questions/23230250/set-email-priority-with-sendgrid-api
                mailMessage.AddHeader("Priority", "Non-Urgent");
                mailMessage.AddHeader("Importance", "Low");

                // https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmail/2bb19f1b-b35e-4966-b1cb-1afd044e83ab
                mailMessage.AddHeader("X-Priority", "5");
                mailMessage.AddHeader("X-MSMail-Priority", "Low");
                break;
        }

        if (!string.IsNullOrEmpty(email.Message.PlainTextAlternativeBody))
        {
            mailMessage.PlainTextContent = email.Message.PlainTextAlternativeBody;
        }

        if (email.Message.Attachments.Any())
        {
            foreach (var attachment in email.Message.Attachments)
            {
                var sendGridAttachment = await ConvertAttachmentAsync(attachment);
                mailMessage.AddAttachment(
                    sendGridAttachment.Filename,
                    sendGridAttachment.Content,
                    sendGridAttachment.Type,
                    sendGridAttachment.Disposition,
                    sendGridAttachment.ContentId);
            }
        }

        var sendGridResponse = await sendGridClient.SendEmailAsync(mailMessage, cancellation.GetValueOrDefault());

        var sendResponse = new NotificationResult();

        if (sendGridResponse.Headers.TryGetValues(
            "X-Message-ID",
            out var messageIds))
        {
            sendResponse.MessageId = messageIds.FirstOrDefault();
        }

        if (IsHttpSuccess((int)sendGridResponse.StatusCode))
        {
            return sendResponse;
        }

        var errorsList = new List<string>();

        errorsList.Add($"{sendGridResponse.StatusCode}");
        var messageBodyDictionary = await sendGridResponse.DeserializeResponseBodyAsync(sendGridResponse.Body);

        if (messageBodyDictionary.ContainsKey("errors"))
        {
            var errors = messageBodyDictionary["errors"];

            foreach (var error in errors)
            {
                errorsList.Add($"{error}");
            }
        }

        return NotificationResult.Failed(errorsList.ToArray());
    }

    private EmailAddress ConvertAddress(Address address)
    {
        return new EmailAddress(address.Email, address.Name);
    }

    private async Task<SendGridAttachment> ConvertAttachmentAsync(Abstractions.Smtp.Attachment attachment)
    {
        return new SendGridAttachment
        {
            Content = await GetAttachmentBase64StringAsync(attachment.Stream),
            Filename = attachment.Filename,
            Type = attachment.ContentType
        };
    }

    private async Task<string> GetAttachmentBase64StringAsync(Stream stream)
    {
        using (var ms = new MemoryStream())
        {
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    private bool IsHttpSuccess(int statusCode)
    {
        return statusCode >= 200 && statusCode < 300;
    }
}
