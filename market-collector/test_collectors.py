from app.collectors.crypto import get_all_cyptos_quotes
from app.collectors.stocks import get_quote
from app.indicators.technical import get_technical_analysis

print(get_quote("PETR4.SA"))
print("-----")

print(get_all_cyptos_quotes())
print("-----")

print(get_technical_analysis("PETR4.SA"))