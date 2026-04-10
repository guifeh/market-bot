using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketBot.Infrastructure.Data.Configurations;

public class TechnicalAnalysisConfiguration : IEntityTypeConfiguration<TechnicalAnalysis>
{
    public void Configure(EntityTypeBuilder<TechnicalAnalysis> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Sma20).HasPrecision(18, 8);
        builder.Property(t => t.Sma50).HasPrecision(18, 8);
        builder.Property(t => t.Ema9).HasPrecision(18, 8);
        builder.Property(t => t.Rsi14).HasPrecision(10, 4);
        builder.Property(t => t.Macd).HasPrecision(18, 8);
        builder.Property(t => t.MacdSignal).HasPrecision(18, 8);
        builder.Property(t => t.MacdHist).HasPrecision(18, 8);
        builder.Property(t => t.BbUpper).HasPrecision(18, 8);
        builder.Property(t => t.BbMid).HasPrecision(18, 8);
        builder.Property(t => t.BbLower).HasPrecision(18, 8);

        builder.Property(t => t.Signals).HasColumnType("jsonb");

        builder.HasOne(t => t.Asset)
               .WithMany(a => a.Analyses)
               .HasForeignKey(t => t.AssetId);
    }
}
