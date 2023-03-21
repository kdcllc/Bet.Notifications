using Bet.Notifications.Abstractions.TemplateRenderers;
using Bet.Notifications.Razor;
using Bet.Notifications.Razor.Options;
using Bet.Notifications.Razor.Repository;
using Bet.Notifications.Razor.Repository.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class RazorNotificationBuilderExtensions
{
    /// <summary>
    /// Adds Razor Template Renderer.
    /// The default is based on the directory root provider.
    /// </summary>
    /// <param name="builder">The <see cref="INotificationBuilder"/> instance.</param>
    /// <param name="configure">To configure the options for the render.</param>
    /// <returns></returns>
    public static INotificationBuilder AddRazorTemplateRenderer(
        this INotificationBuilder builder,
        Action<RazorTemplateRendererOptions, IServiceProvider>? configure = null)
    {
        // regiter razor options.
        builder.Services.AddChangeTokenOptions<RazorTemplateRendererOptions>(
            nameof(RazorTemplateRendererOptions),
            builder.Name,
            (o, c) =>
            {
                configure?.Invoke(o, c);

                if (configure == null)
                {
                    o.RootDirectory = Directory.GetCurrentDirectory();
                }
            });

        builder.Services.AddSingleton<ITemplateRenderer, RazorTemplateRenderer>(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<RazorTemplateRendererOptions>>();
            return new RazorTemplateRenderer(builder.Name, options);
        });

        return builder;
    }

    public static INotificationBuilder AddInMemoryRazorTemplateRenderer(
        this INotificationBuilder builder)
    {
        builder.Services.AddDbContext<TemplateDbContext>(
                                    options => options.UseInMemoryDatabase("TemplateDb"),
                                    ServiceLifetime.Transient,
                                    ServiceLifetime.Transient);

        builder.Services.AddTransient<ITemplateRepository, TemplateRepository>();

        // RazorTemplateDbRenderer
        builder.Services.AddSingleton<ITemplateRenderer, RazorTemplateDbRenderer>(sp =>
        {
            var repo = sp.GetRequiredService<ITemplateRepository>();
            var context = sp.GetRequiredService<TemplateDbContext>();
            context.Database.EnsureCreated();
            return new RazorTemplateDbRenderer(builder.Name, new RepositoryRazorLightProject(repo));
        });

        return builder;
    }
}
