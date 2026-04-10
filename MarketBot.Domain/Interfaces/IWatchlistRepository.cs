using MarketBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Interfaces;

public interface IWatchlistRepository
{
    Task<List<Watchlist>> GetAllAsync();
    Task<List<string>> GetTickersAsync();
    Task<Watchlist> AddAsync(Watchlist watchlist);
    Task RemoveAsync(int assetId);
    Task<bool> ExistsAsync(int assetId);
}
