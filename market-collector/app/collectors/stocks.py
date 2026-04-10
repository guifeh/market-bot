import yfinance as yf
import pandas as pd
from datetime import datetime

BR_STOCKS = ["PETR4.SA", "VALE3.SA", "ITUB4.SA", "BBDC4.SA", "WEGE3.SA"]
US_STOCKS = ["AAPL", "NVDA", "MSFT", "TSLA", "AMZN"]

def get_quote(ticker: str) -> dict:
    """Retorna cotação atual + dados fundamentalistas básicos."""
    asset = yf.Ticker(ticker)
    info = asset.info

    return {
        "ticker": ticker,
        "name": info.get("longName") or info.get("shortName"),
        "price": info.get("currentPrice") or info.get("regularMarketPrice"),
        "change_pct": info.get("regularMarketChangePercent"),
        "volume": info.get("regularMarketVolume"),
        "market_cap": info.get("marketCap"),
        # Fundamentalista básico
        "pe_ratio": info.get("trailingPE"),
        "ev_ebitda": info.get("enterpriseToEbitda"),
        "roe": info.get("returnOnEquity"),
        "dividend_yield": info.get("dividendYield"),
        "currency": info.get("currency"),
        "timestamp": datetime.utcnow().isoformat(),
    }

def get_history(ticker: str, period: str = "3mo", interval: str = "1d") -> pd.DataFrame:
    """Retorna histórico OHLCV — usado pra calcular indicadores."""
    df = yf.download(ticker, period=period, interval=interval, progress=False)
    df.dropna(inplace=True)
    return df

def get_all_quotes() ->list[dict]:
    quotes=[]
    for ticker in BR_STOCKS + US_STOCKS:
        try:
            quotes.append(get_quote(ticker))
        except Exception as e:
            print(f"Error fetching quote for {ticker}: {e}")
    return quotes