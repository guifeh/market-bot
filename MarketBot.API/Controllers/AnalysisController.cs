using MarketBot.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketBot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController(MarketAnalysisService analysisService) : ControllerBase
{
    [HttpPost("{ticker}")]
    public async Task<IActionResult> Analyse(string ticker)
    {
        try
        {
            var result = await analysisService.AnalyseAssetAsync(ticker);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}