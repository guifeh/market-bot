using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using MarketBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketBot.Infrastructure.Repositories;

public class AssetRepository(AppDbContext context) : IAssetRepository
{
    public async Task<Asset?> GetByTickerAsync(string ticker) =>
        await context.Assets.FirstOrDefaultAsync(a => a.Ticker == ticker);

    public async Task<List<Asset>> GetAllAsync() => 
        await context.Assets.ToListAsync();

    public async Task<Asset> CreateAsync(Asset asset)
    {
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        return asset;
    }
    public async Task<bool> ExistsAsync(string ticker) => 
        await context.Assets.AnyAsync(a => a.Ticker == ticker);
}
