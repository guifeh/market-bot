using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Entities;

public class Quote
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public decimal Price { get; set; }
    public decimal? ChangePct { get; set; }
    public long? Volume { get; set; }
    public decimal? MarketCap { get; set; }
    public decimal? PeRatio { get; set; }
    public decimal? EvEbitda { get; set; }
    public decimal? Roe { get; set; }
    public decimal? DividendYield { get; set; }
    public DateTime CollectedAt { get; set; }
    public Asset Asset { get; set; } = null!;
}
