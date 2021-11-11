namespace Bet.Notifications.Abstractions.Smtp;

/// <summary>
/// Represents a class that contains information for a mail message attachment.
/// </summary>
public class Attachment
{
    /// <summary>
    /// Gets or sets the attachment filename.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the attachment file stream.
    /// </summary>
    public Stream? Stream { get; set; }

    /// <summary>
    /// Content type of the file based on the extension of the file.
    /// </summary>
    public string? ContentType => MimeTypeLookup.GetMimeType(Filename ?? "image.jpeg");
}
