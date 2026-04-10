using MarketBot.Domain.Entities;
using MarketBot.Domain.Interfaces;
using MarketBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Infrastructure.Repositories;

public class QuoteRepository(AppDbContext context) : IQuouteRepository 
{
    public async Task<Quote?> GetLatestByAssetIdAsync(int assetId) =>
        await context.Quotes
            .Where(q => q.AssetId == assetId)
            .OrderByDescending(q => q.CollectedAt)
            .FirstOrDefaultAsync();

    public async Task<List<Quote>> GetHistoryByAssetIdAsync(int assetId, int limit = 30) =>
        await context.Quotes
            .Where(q => q.AssetId == assetId)
            .OrderByDescending(q => q.CollectedAt)
            .Take(limit)
            .ToListAsync();

    public async Task<Quote> CreateAsync(Quote quote)
    {
        context.Quotes.Add(quote);
        await context.SaveChangesAsync();
        return quote;
    }
}
