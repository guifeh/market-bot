using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketBot.Infrastructure.Data.Configurations;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Price).HasPrecision(18,8);
        builder.Property(q => q.ChangePct).HasPrecision(10, 4);
        builder.Property(q => q.MarketCap).HasPrecision(24, 2);
        builder.Property(q => q.PeRatio).HasPrecision(10, 4);
        builder.Property(q => q.EvEbitda).HasPrecision(10, 4);
        builder.Property(q => q.Roe).HasPrecision(10, 4);
        builder.Property(q => q.DividendYield).HasPrecision(10, 4);

        builder.HasOne(q => q.Asset)
                .WithMany(a => a.Quotes)
                .HasForeignKey(q => q.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
