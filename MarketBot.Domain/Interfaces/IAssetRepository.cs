using MarketBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketBot.Domain.Interfaces;

public interface IAssetRepository
{
    Task<Asset> GetByTickerAsync(string ticker);
    Task<List<Asset>> GetAllAsync();
    Task<Asset> CreateAsync(Asset asset);
    Task<bool> ExistsAsync(string ticker);
}
