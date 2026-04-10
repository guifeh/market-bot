using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketBot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchlistController(IWatchlistRepository watchlistRepo, IAssetRepository assetRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await watchlistRepo.GetAllAsync());

    [HttpGet("tickers")]
    public async Task<IActionResult> GetTickers() =>
        Ok(await watchlistRepo.GetTickersAsync());

    [HttpPost("{ticker}")]
    public async Task<IActionResult> Add(string ticker)
    {
        var asset = await assetRepo.GetByTickerAsync(ticker.ToUpper());
        if (asset is null) return NotFound("Ativo não encontrado — cadastre primeiro em /api/assets");

        if (await watchlistRepo.ExistsAsync(asset.Id))
            return Conflict("Ativo já está na watchlist");

        var item = new Watchlist { AssetId = asset.Id };
        await watchlistRepo.AddAsync(item);
        return Ok(new { message = $"{ticker.ToUpper()} adicionado à watchlist" });
    }

    [HttpDelete("{ticker}")]
    public async Task<IActionResult> Remove(string ticker)
    {
        var asset = await assetRepo.GetByTickerAsync(ticker.ToUpper());
        if (asset is null) return NotFound();

        await watchlistRepo.RemoveAsync(asset.Id);
        return NoContent();
    }
}