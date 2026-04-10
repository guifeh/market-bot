using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using MarketBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Infrastructure.Repositories;

public class WatchlistRepository(AppDbContext context) : IWatchlistRepository
{
    public async Task<List<Watchlist>> GetAllAsync() =>
        await context.Watchlists.Include(w => w.Asset).ToListAsync();

    public async Task<List<string>> GetTickersAsync() =>
        await context.Watchlists
             .Include(w => w.Asset)
             .Select(w => w.Asset.Ticker)
             .ToListAsync();

    public async Task<Watchlist> AddAsync(Watchlist watchlist)
    {
        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();
        return watchlist;
    }

    public async Task RemoveAsync(int assetId)
    {
        var watchlist = await context.Watchlists.FirstOrDefaultAsync(w => w.AssetId == assetId);
        if (watchlist != null)
        {
            context.Watchlists.Remove(watchlist);
            await context.SaveChangesAsync();
        }
    }
    public async Task<bool> ExistsAsync(int assetId) =>
        await context.Watchlists.AnyAsync(w => w.AssetId == assetId);
}
