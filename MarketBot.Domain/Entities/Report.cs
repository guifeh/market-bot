using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public string Content { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Asset Asset { get; set; } = null!;
}
