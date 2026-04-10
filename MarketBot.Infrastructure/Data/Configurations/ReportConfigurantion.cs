using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Infrastructure.Data.Configurations;

public class ReportConfigurantion : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Content).IsRequired();
        builder.Property(r => r.Type).IsRequired().HasMaxLength(20);

        builder.HasOne(r => r.Asset)
               .WithMany(a => a.Reports)
               .HasForeignKey(r => r.AssetId);
    }
}
