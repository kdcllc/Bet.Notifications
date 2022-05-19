using Bet.Notifications.Razor.Repository.EntityFrameworkCore.EntityConfigurations;

using Microsoft.EntityFrameworkCore;

namespace Bet.Notifications.Razor.Repository.EntityFrameworkCore;

#nullable disable
public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {
    }

    public DbSet<TemplateItem> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TemplateItemConfiguration());

        modelBuilder.Entity<TemplateItem>().HasData(
            new TemplateItem
            {
                Id = 1,
                Name = "testLayout",
                Content = @"
                        <html>
                            <h1>@ViewBag.Title</h1>
                            @RenderBody()
                        </html>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Version = 1,
            },
            new TemplateItem
            {
                Id = 2,
                Name = "testTemplate",
                Content = @"
                    @{
                        Layout = ""testLayout""; //This is a name of your layout in database
                     }
                    <body> Hello, my name is @Model.Name and I am @Model.Age </body>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Version = 1,
            });
    }
}
