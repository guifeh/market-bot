namespace MarketBot.Domain.Entities;

public class TechnicalAnalysis
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public decimal? Sma20 { get; set; }
    public decimal? Sma50 { get; set; }
    public decimal? Ema9 { get; set; }
    public decimal? Rsi14 { get; set; }
    public decimal? Macd { get; set; }
    public decimal? MacdSignal { get; set; }
    public decimal? MacdHist { get; set; }
    public decimal? BbUpper { get; set; }
    public decimal? BbMid { get; set; }
    public decimal? BbLower { get; set; }
    public List<string> Signals { get; set; } = [];
    public DateTime AnalysedAt { get; set; }
    public Asset Asset { get; set; } = null!;
}