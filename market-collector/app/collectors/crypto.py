import requests
from datetime import datetime

CRYPTO_SYMBOLS = ["BTCUSDT", "ETHUSDT", "SOLUSDT"]

def get_crypto_quote(symbol: str) -> dict:
    ticker_url = f"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}"
    response = requests.get(ticker_url, timeout=10)
    response.raise_for_status()
    data = response.json()

    return {
        "ticker": symbol,
        "name": symbol.replace("USDT", ""),
        "price": float(data["lastPrice"]),
        "change_pct": float(data["priceChangePercent"]),
        "volume": float(data["volume"]),
        "high_24h": float(data["highPrice"]),
        "low_24h": float(data["lowPrice"]),
        "currency": "USDT",
        "timestamp": datetime.utcnow().isoformat(),
    }

def get_all_cyptos_quotes() -> list[dict]:
    quotes = []
    for symbol in CRYPTO_SYMBOLS:
        try:
            quotes.append(get_crypto_quote(symbol))
        except Exception as e:
            print(f"Error fetching quote for {symbol}: {e}")
    return quotes