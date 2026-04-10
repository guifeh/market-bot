using MarketBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Interfaces;

public interface IQuouteRepository
{
    Task<Quote> GetLatestByAssetIdAsync(int assetId);
    Task<List<Quote>> GetHistoryByAssetIdAsync(int assetId, int limit = 30);
    Task<Quote> CreateAsync(Quote quote);
}
