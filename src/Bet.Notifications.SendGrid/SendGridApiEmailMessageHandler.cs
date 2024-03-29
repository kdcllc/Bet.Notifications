﻿using Bet.Notifications.Abstractions;
using Bet.Notifications.Abstractions.Smtp;
using Bet.Notifications.SendGrid.Options;

using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

using SendGridAttachment = SendGrid.Helpers.Mail.Attachment;

namespace Bet.Notifications.SendGrid;

public class SendGridApiEmailMessageHandler : IEmailMessageHandler
{
    private readonly SendGridOptions _options;
    private readonly ISendGridClient _client;

    // ISendGridClient
    public SendGridApiEmailMessageHandler(
        string name,
        ISendGridClient client,
        IOptionsMonitor<SendGridOptions> optionsMonitor)
    {
        Name = name;
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = optionsMonitor.Get(name);
    }

    public string Name { get; }

    public async Task<NotificationResult> SendAsync(EmailMessage email, CancellationToken? cancellation = null)
    {
        var mailMessage = new SendGridMessage();
        mailMessage.SetSandBoxMode(_options.IsSandBoxMode);

        mailMessage.SetFrom(ConvertAddress(email.From));

        if (email.To.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddTos(email.To.Select(ConvertAddress).ToList());
        }

        if (email.Cc.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddCcs(email.Cc.Select(ConvertAddress).ToList());
        }

        if (email.Bcc.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            mailMessage.AddBccs(email.Bcc.Select(ConvertAddress).ToList());
        }

        if (email.ReplyTo.Any(a => !string.IsNullOrWhiteSpace(a.Email)))
        {
            // SendGrid does not support multiple ReplyTo addresses
            mailMessage.SetReplyTo(email.ReplyTo.Select(ConvertAddress).First());
        }

        if (_options.UseOpenTracking)
        {
            mailMessage.SetOpenTracking(true);
        }

        if (_options.UseClickTracking)
        {
            mailMessage.SetClickTracking(true, true);
        }

        if (email.Tags.Any())
        {
            mailMessage.AddCategories(email.Tags.ToList());
        }

        mailMessage.SetSubject(email.Subject);

        if (email.Headers.Any())
        {
            mailMessage.AddHeaders(email.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        if (email.IsHtml)
        {
            mailMessage.HtmlContent = email.Body;
        }
        else
        {
            mailMessage.PlainTextContent = email.Body;
        }

        switch (email.Priority)
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

        if (!string.IsNullOrEmpty(email.PlainTextAltBody))
        {
            mailMessage.PlainTextContent = email.PlainTextAltBody;
        }

        if (email.Attachments.Any())
        {
            foreach (var attachment in email.Attachments)
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

        try
        {
            var sendGridResponse = await _client.SendEmailAsync(mailMessage, cancellation.GetValueOrDefault()).ConfigureAwait(false); ;

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

            var errorsList = new List<string>
            {
                $"{sendGridResponse.StatusCode}"
            };
            var messageBodyDictionary = await sendGridResponse.DeserializeResponseBodyAsync();

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
        catch (Exception ex)
        {
            if (_options.ThrowException)
            {
                throw;
            }

            return NotificationResult.Failed(ex?.Message ?? string.Empty, ex?.InnerException?.Message ?? string.Empty);
        }
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
