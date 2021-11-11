namespace Microsoft.Extensions.DependencyInjection;

public static class SendGridNotificationBuilderExtensions
{
    public static INotificationBuilder AddSendGridSender(this INotificationBuilder builder)
    {
        return builder;
    }
}
