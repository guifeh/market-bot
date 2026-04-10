using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Infrastructure.Data.Configurations;

public class WatchlistConfigurantion : IEntityTypeConfiguration<Watchlist>
{
    public void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        builder.HasKey(w => w.Id);
        builder.HasIndex(w => w.AssetId).IsUnique();

        builder.HasOne(w => w.Asset)
               .WithMany(a => a.Watchlists)
               .HasForeignKey(w => w.AssetId);
    }
}
