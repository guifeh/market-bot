using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketBot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController(IAssetRepository assetRepository, IQuouteRepository quouteRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await assetRepository.GetAllAsync());

    [HttpGet("{ticker}")]
    public async Task<IActionResult> GetByTicker(string ticker)
    {
        var asset = await assetRepository.GetByTickerAsync(ticker.ToUpper());
        return asset is null ? NotFound() : Ok(asset);
    }

    [HttpGet("{ticker}/quote")]
    public async Task<IActionResult> GetLatestQuote(string ticker)
    {
        var asset = await assetRepository.GetByTickerAsync(ticker.ToUpper());
        if (asset is null) return NotFound("Ativo não encontrado");

        var quote = await quouteRepository.GetLatestByAssetIdAsync(asset.Id);
        return quote is null ? NotFound("Sem cotações para este ativo") : Ok(quote);
    }

    [HttpGet("{ticker}/history")]
    public async Task<IActionResult> GetHistory(string ticker, [FromQuery] int limit)
    {
        var asset = await assetRepository.GetByTickerAsync(ticker.ToUpper());
        if (asset is null) return NotFound("Ativo não encontrado");

        var history = await quouteRepository.GetHistoryByAssetIdAsync(asset.Id, limit);
        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        if (await assetRepository.ExistsAsync(request.Ticker.ToUpper()))
            return Conflict("Ativo já cadastrado");

        var asset = new Asset
        {
            Ticker = request.Ticker.ToUpper(),
            Name = request.Name,
            Type = request.Type,
            Currency = request.Currency,
        };

        var created = await assetRepository.CreateAsync(asset);
        return CreatedAtAction(nameof(GetByTicker), new { ticker = created.Ticker }, created);
    }
}

public record CreateAssetRequest(string Ticker, string Name, string Type, string Currency);