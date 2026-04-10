using MarketBot.API.Services;
using MarketBot.Domain.Interfaces;
using MarketBot.Infrastructure.Data;
using MarketBot.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpClient("PythonService", client =>
{
    client.BaseAddress = new Uri("http://localhost:8000");
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IQuouteRepository, QuoteRepository>();
builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<MarketAnalysisService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();
app.Run();
