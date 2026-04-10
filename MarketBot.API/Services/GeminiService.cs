using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

namespace MarketBot.API.Services;

public class GeminiService(IConfiguration config) : IGeminiService
{
    public async Task<string> AnalyseAsync(string prompt)
    {
        var apiKey = config["Gemini:ApiKey"]!;

        var googleAI = new GoogleAI(apiKey);
        var model = googleAI.GenerativeModel(Model.Gemini25FlashLite);

        var response = await model.GenerateContent(prompt);
        return response.Text ?? "Sem resposta gerada.";
    }
}