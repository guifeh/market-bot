from fastapi import FastAPI
from app.routers.market import router as market_router

app = FastAPI(title="Market Collector", version="1.0.0")

app.include_router(market_router)

@app.get("/health")
def health_check():
    return {"status": "healthy"}