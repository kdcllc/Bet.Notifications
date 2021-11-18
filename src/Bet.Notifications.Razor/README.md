# Bet.Notifications.Razor

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://raw.githubusercontent.com/kdcllc/Bet.Notifications.Razor/master/LICENSE)
![master workflow](https://github.com/kdcllc/Bet.Notifications/actions/workflows/master.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Bet.Notifications.Razor.svg)](https://www.nuget.org/packages?q=Bet.Notifications.Razor)
![Nuget](https://img.shields.io/nuget/dt/Bet.Notifications.Razor)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/bet-notifications/shield/Bet.Notifications.Razor/latest)](https://f.feedz.io/kdcllc/bet-notifications/packages/Bet.Notifications.Razor/latest/download)

> The second letter in the Hebrew alphabet is the ב bet/beit. Its meaning is "house". In the ancient pictographic Hebrew it was a symbol resembling a tent on a landscape.

_Note: Pre-release packages are distributed via [feedz.io](https://f.feedz.io/kdcllc/bet-notifications/nuget/index.json)._

This goal of this repo is to provide with a reusable functionality for developing Microservices with Docker and Kubernetes.

## Hire me

Please send [email](mailto:kingdavidconsulting@gmail.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Usage

This library supports Repository Razor For templates.

```csharp
    services.AddEmailConfigurator()
        .AddInMemoryRazorTemplateRenderer()
        .AddFileSystemEmailMessageHandler();
```

## Implementing Custom Entity Framework Repository

1. Add Repository Item to the `CustomDbContext`

```csharp
public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
    {
    }

    public DbSet<TemplateItem> EmailTemplates { get; set; }
}
```

2. Create Implementation of `ITemplateRepository`

```csharp
public class RazorTemplateRepository : ITemplateRepository
{
    private readonly MarketingContext _context;

    public RazorTemplateRepository(MarketingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<TemplateItem> GetSingleAsync(string templateName, CancellationToken cancellation = default)
    {
        return _context.EmailTemplates.FirstOrDefaultAsync(x => x.Name == templateName, cancellation);
    }

    public async Task<IEnumerable<TemplateItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmailTemplates.ToListAsync(cancellationToken);
    }
}
```

In order for the projects to use this library please add this to you project file:

```xml

  <PropertyGroup>
    <PreserveCompilationContext>true</PreserveCompilationContext>
  </PropertyGroup>

```
