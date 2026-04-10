namespace MarketBot.API.Services;

public interface IGeminiService
{
    Task<string> AnalyseAsync(string prompt);
}
