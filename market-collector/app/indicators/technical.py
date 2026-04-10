import pandas as pd
import pandas_ta as ta
from app.collectors.stocks import get_history

def calculate_indicators(df: pd.DataFrame) -> dict:
    close = df["Close"].squeeze()

    sma_20 = ta.sma(close, length=20)
    sma_50 = ta.sma(close, length=50)
    ema_9  = ta.ema(close, length=9)
    rsi    = ta.rsi(close, length=14)

    macd_df   = ta.macd(close, fast=12, slow=26, signal=9)
    bbands_df = ta.bbands(close, length=20, std=2)

    # Pega os nomes reais das colunas em vez de hardcodar
    macd_col   = next((c for c in macd_df.columns if c.startswith("MACD_")),  None) if macd_df   is not None else None
    signal_col = next((c for c in macd_df.columns if c.startswith("MACDs_")), None) if macd_df   is not None else None
    hist_col   = next((c for c in macd_df.columns if c.startswith("MACDh_")), None) if macd_df   is not None else None
    bb_upper_col = next((c for c in bbands_df.columns if c.startswith("BBU")), None) if bbands_df is not None else None
    bb_mid_col   = next((c for c in bbands_df.columns if c.startswith("BBM")), None) if bbands_df is not None else None
    bb_lower_col = next((c for c in bbands_df.columns if c.startswith("BBL")), None) if bbands_df is not None else None

    def last(series):
        if series is None or series.empty:
            return None
        val = series.dropna()
        return round(float(val.iloc[-1]), 4) if not val.empty else None

    return {
        "sma_20": last(sma_20),
        "sma_50": last(sma_50),
        "ema_9":  last(ema_9),
        "rsi_14": last(rsi),
        "macd":        last(macd_df[macd_col])   if macd_col   else None,
        "macd_signal": last(macd_df[signal_col]) if signal_col else None,
        "macd_hist":   last(macd_df[hist_col])   if hist_col   else None,
        "bb_upper": last(bbands_df[bb_upper_col]) if bb_upper_col else None,
        "bb_mid":   last(bbands_df[bb_mid_col])   if bb_mid_col   else None,
        "bb_lower": last(bbands_df[bb_lower_col]) if bb_lower_col else None,
    }

def get_technical_analysis(ticker: str) -> dict:
    df = get_history(ticker)

    if df.empty:
        return {"ticker": ticker, "error": "Sem dados históricos"}

    indicators = calculate_indicators(df)

    rsi        = indicators.get("rsi_14")
    macd       = indicators.get("macd")
    macd_signal = indicators.get("macd_signal")
    price      = float(df["Close"].squeeze().iloc[-1])
    sma_50     = indicators.get("sma_50")

    signals = []

    if rsi:
        if rsi < 30:
            signals.append("RSI sobrevendido — possível reversão de alta")
        elif rsi > 70:
            signals.append("RSI sobrecomprado — possível reversão de baixa")
        else:
            signals.append(f"RSI neutro ({rsi})")

    if macd and macd_signal:
        if macd > macd_signal:
            signals.append("MACD acima do sinal — momentum de alta")
        else:
            signals.append("MACD abaixo do sinal — momentum de baixa")

    if sma_50:
        if price > sma_50:
            signals.append("Preço acima da SMA50 — tendência de alta")
        else:
            signals.append("Preço abaixo da SMA50 — tendência de baixa")

    return {
        "ticker": ticker,
        "indicators": indicators,
        "signals": signals,
    }