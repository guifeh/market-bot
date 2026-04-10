using System.Collections;

namespace MarketBot.Domain.Entities;

public class Asset
{
    public int Id { get; set; }
    public string Ticker { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    public ICollection<Quote> Quotes { get; set; } = [];
    public ICollection<TechnicalAnalysis> Analyses { get; set; } = [];
    public ICollection<Watchlist> Watchlists { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
}
