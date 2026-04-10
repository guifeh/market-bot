using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using MarketBot.Infrastructure.Data;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MarketBot.API.Services;

public class MarketAnalysisService(
    IGeminiService gemini,
    IAssetRepository assetRepo,
    IQuouteRepository quoteRepo,
    AppDbContext context,
    IHttpClientFactory httpClientFactory)
{
    public async Task<TechnicalAnalysis> AnalyseAssetAsync(string ticker)
    {
        var asset = await assetRepo.GetByTickerAsync(ticker.ToUpper())
            ?? throw new Exception($"Ativo {ticker} não cadastrado");

        var client = httpClientFactory.CreateClient("PythonService");

        var quoteJson = await client.GetStringAsync($"/market/stocks/{ticker}");
        var analysisJson = await client.GetStringAsync($"/market/stocks/{ticker}/analysis");

        var quoteData = JsonSerializer.Deserialize<JsonElement>(quoteJson);
        var analysisData = JsonSerializer.Deserialize<JsonElement>(analysisJson);

        var price = quoteData.GetProperty("price").GetDecimal();
        var changePct = quoteData.TryGetProperty("change_pct", out var cpEl) && cpEl.ValueKind != JsonValueKind.Null ? (decimal?)cpEl.GetDouble() : null;
        var volume = quoteData.TryGetProperty("volume", out var volEl) && volEl.ValueKind != JsonValueKind.Null ? (long?)Math.Round(volEl.GetDouble()) : null;
        var marketCap = quoteData.TryGetProperty("market_cap", out var mcEl) && mcEl.ValueKind != JsonValueKind.Null ? (decimal?)mcEl.GetDouble() : null;
        var peRatio = quoteData.TryGetProperty("pe_ratio", out var peEl) && peEl.ValueKind != JsonValueKind.Null ? (decimal?)peEl.GetDouble() : null;
        var evEbitda = quoteData.TryGetProperty("ev_ebitda", out var evEl) && evEl.ValueKind != JsonValueKind.Null ? (decimal?)evEl.GetDouble() : null;
        var roe = quoteData.TryGetProperty("roe", out var roeEl) && roeEl.ValueKind != JsonValueKind.Null ? (decimal?)roeEl.GetDouble() : null;
        var divYield = quoteData.TryGetProperty("dividend_yield", out var dyEl) && dyEl.ValueKind != JsonValueKind.Null ? (decimal?)dyEl.GetDouble() : null;

        var quote = new Quote
        {
            AssetId = asset.Id,
            Price = price,
            ChangePct = changePct,
            Volume = volume,
            MarketCap = marketCap,
            PeRatio = peRatio,
            EvEbitda = evEbitda,
            Roe = roe,
            DividendYield = divYield,
            CollectedAt = DateTime.UtcNow,
        };
        await quoteRepo.CreateAsync(quote);

        var indicators = analysisData.GetProperty("indicators");
        var signals = analysisData.GetProperty("signals")
            .EnumerateArray()
            .Select(s => s.GetString()!)
            .ToList();

        var prompt = BuildPrompt(ticker, asset.Name, quoteData, indicators, signals);

        var aiResponse = await gemini.AnalyseAsync(prompt);

        var analysis = new TechnicalAnalysis
        {
            AssetId = asset.Id,
            Sma20 = GetDecimal(indicators, "sma_20"),
            Sma50 = GetDecimal(indicators, "sma_50"),
            Ema9 = GetDecimal(indicators, "ema_9"),
            Rsi14 = GetDecimal(indicators, "rsi_14"),
            Macd = GetDecimal(indicators, "macd"),
            MacdSignal = GetDecimal(indicators, "macd_signal"),
            MacdHist = GetDecimal(indicators, "macd_hist"),
            BbUpper = GetDecimal(indicators, "bb_upper"),
            BbMid = GetDecimal(indicators, "bb_mid"),
            BbLower = GetDecimal(indicators, "bb_lower"),
            Signals = signals,
            AnalysedAt = DateTime.UtcNow,
        };

        context.TechnicalAnalyses.Add(analysis);
        await context.SaveChangesAsync();

        var report = new Report
        {
            AssetId = asset.Id,
            Content = aiResponse,
            Type = "ondemand",
            CreatedAt = DateTime.UtcNow,
        };
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        return analysis;
    }

    private static string BuildPrompt(
        string ticker,
        string name,
        JsonElement quote,
        JsonElement indicators,
        List<string> signals)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Você é um analista financeiro experiente. Analise o ativo abaixo com base nos dados técnicos e fundamentalistas fornecidos.");
        sb.AppendLine("Seja objetivo e direto. Estruture sua resposta em: Resumo, Análise Técnica, Análise Fundamentalista, Riscos e Oportunidades, e Conclusão.");
        sb.AppendLine();
        sb.AppendLine($"## Ativo: {name} ({ticker})");
        sb.AppendLine();
        sb.AppendLine("### Cotação Atual");
        sb.AppendLine($"- Preço: {(quote.TryGetProperty("price", out var p) && p.ValueKind != JsonValueKind.Null ? p.GetDecimal().ToString() : "N/A")}");
        sb.AppendLine($"- Variação: {(quote.TryGetProperty("change_pct", out var cp) && cp.ValueKind != JsonValueKind.Null ? cp.GetDecimal().ToString() : "N/A")}%");
        sb.AppendLine($"- Volume: {(quote.TryGetProperty("volume", out var v) && v.ValueKind != JsonValueKind.Null ? v.GetDouble().ToString() : "N/A")}");
        sb.AppendLine();
        sb.AppendLine("### Indicadores Técnicos");
        sb.AppendLine($"- RSI (14): {GetDecimal(indicators, "rsi_14")}");
        sb.AppendLine($"- MACD: {GetDecimal(indicators, "macd")} | Sinal: {GetDecimal(indicators, "macd_signal")}");
        sb.AppendLine($"- SMA 20: {GetDecimal(indicators, "sma_20")} | SMA 50: {GetDecimal(indicators, "sma_50")}");
        sb.AppendLine($"- Bollinger: {GetDecimal(indicators, "bb_lower")} / {GetDecimal(indicators, "bb_mid")} / {GetDecimal(indicators, "bb_upper")}");
        sb.AppendLine();
        sb.AppendLine("### Indicadores Fundamentalistas");
        sb.AppendLine($"- P/L: {(quote.TryGetProperty("pe_ratio", out var pe) && pe.ValueKind != JsonValueKind.Null ? pe.GetDecimal().ToString() : "N/A")}");
        sb.AppendLine($"- EV/EBITDA: {(quote.TryGetProperty("ev_ebitda", out var ev) && ev.ValueKind != JsonValueKind.Null ? ev.GetDecimal().ToString() : "N/A")}");
        sb.AppendLine($"- ROE: {(quote.TryGetProperty("roe", out var roe) && roe.ValueKind != JsonValueKind.Null ? roe.GetDecimal().ToString() : "N/A")}");
        sb.AppendLine($"- Dividend Yield: {(quote.TryGetProperty("dividend_yield", out var dy) && dy.ValueKind != JsonValueKind.Null ? dy.GetDecimal().ToString() : "N/A")}%");
        sb.AppendLine();
        sb.AppendLine("### Sinais Identificados");
        signals.ForEach(s => sb.AppendLine($"- {s}"));
        sb.AppendLine();
        sb.AppendLine("Forneça sua análise completa:");

        return sb.ToString();
    }

    private static decimal? GetDecimal(JsonElement element, string key) =>
        element.TryGetProperty(key, out var val) && val.ValueKind != JsonValueKind.Null
            ? val.GetDecimal()
            : null;
}