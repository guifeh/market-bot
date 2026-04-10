using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketBot.Infrastructure.Data.Configurations;

public class AssestConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Ticker).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Type).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Currency).IsRequired().HasMaxLength(50);
        builder.HasIndex(a => a.Ticker).IsUnique();
    }
}
