using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Entities;

public class Watchlist
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public bool AutoAnalysis { get; set; } = true;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public Asset Asset { get; set; } = null!;
}
