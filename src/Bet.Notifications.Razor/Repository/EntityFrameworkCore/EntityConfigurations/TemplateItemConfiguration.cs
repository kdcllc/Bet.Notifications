using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bet.Notifications.Razor.Repository.EntityFrameworkCore.EntityConfigurations;

public class TemplateItemConfiguration : IEntityTypeConfiguration<TemplateItem>
{
    public void Configure(EntityTypeBuilder<TemplateItem> entity)
    {
        entity.ToTable("RazorEmailTempletes", "email");

        entity.HasKey(x => x.Id).IsClustered();

        entity.Property(x => x.Id)
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();
    }
}
