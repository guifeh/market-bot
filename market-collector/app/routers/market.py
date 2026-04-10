from fastapi import APIRouter, HTTPException
from app.collectors.stocks import get_quote, get_all_quotes
from app.collectors.crypto import get_all_cyptos_quotes, get_crypto_quote
from app.indicators.technical import get_technical_analysis

router = APIRouter(prefix="/market", tags=["Market"])

@router.get("/stocks")
def list_all_stocks():
    return get_all_quotes()

@router.get("/stocks/{ticker}")
def get_stock_quote(ticker: str):
    quote = get_quote(ticker.upper())
    if not quote:
        raise HTTPException(status_code=404, detail="Stock not found")
    return quote

@router.get("/stocks/{ticker}/analysis")
def get_stock_analysis(ticker: str):
    return get_technical_analysis(ticker.upper())
    
@router.get("/crypto")
def list_all_crypto():
    return get_all_cyptos_quotes()

@router.get("/crypto/{symbol}")
def crypto_quote(symbol: str):
    quote = get_crypto_quote(symbol)
    if not quote:
        raise HTTPException(status_code=404, detail="Crypto not found")
    return quote

