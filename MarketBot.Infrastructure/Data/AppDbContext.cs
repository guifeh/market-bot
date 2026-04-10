using MarketBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketBot.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<TechnicalAnalysis> TechnicalAnalyses => Set<TechnicalAnalysis>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
